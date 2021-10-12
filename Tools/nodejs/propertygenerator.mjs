import { access, readFile, writeFile } from 'fs/promises';
import { constants } from 'fs';
import { createInterface } from 'readline';

const useColors = true;
const colReset = '\x1b[0m';
const colRed = '\x1b[31m';
const colGreen = '\x1b[32m';
const colYellow = '\x1b[33m';
const colCyan = '\x1b[36m';

const rl = createInterface({
    input: process.stdin,
    output: process.stdout
});

console.log('Minecraft property generator v2.0');

const args = process.argv.slice(2);

begin(args).then(() => {
    releaseReadline();
});

async function begin(args) {
    let blocksPath;
    if (args.length > 0) {
        blocksPath = args.join('');
    } else {
        blocksPath = await getUserInput('Enter path to blocks.json: ');
    }

    try {
        await access(blocksPath, constants.R_OK);
    } catch {
        console.error(colorText(`Could not access file at ${blocksPath}`, colRed));
        return;
    }

    const receiverModel = {};
    const processedProperties = new Map();

    let blocks;
    try {
        blocks = JSON.parse(await readFile(blocksPath, 'utf-8'));
    } catch (err) {
        console.error(err);
        return;
    }
    const keys = Object.keys(blocks);

    console.log(`Loaded ${keys.length} namespaced blocks`);
    
    let index = -1;
    let singleState = 0;
    for (const key of keys) {
        index++;
        const block = blocks[key];
        
        if (block.properties) {
            
            if (singleState > 0) {
                console.log(`${singleState} blocks had a single state and were skipped`);
                singleState = 0;
            }
            
            const props = Object.keys(block.properties);
            console.log(`[${index}] Block ${colorText(key, colGreen)} has properties ${colorText(props.join(), colCyan)}.`);
            
            let matches = [];
            let cancel = false;
            
            for (const prop of props) { // const prop of ['axis', 'rotation', 'waterlogged'] etc.
                if (processedProperties.has(prop)) {
                    const processedProps = processedProperties.get(prop); // 'axis' => { "log": ["x", "y", "z"] }
                    const propRefs = Object.keys(processedProps);
                    
                    const currentRefs = [];
                    
                    for (const propRef of propRefs) { // const propRef of [ 'log' ]
                        if (props.length === Object.keys(receiverModel[propRef].properties).length &&
                            props.every(x => receiverModel[propRef].properties[x]) &&
                            processedProps[propRef].length === block.properties[prop].length &&
                            processedProps[propRef].every(x => block.properties[prop].includes(x))) { // processedProps['log'] ...
                            currentRefs.push(propRef);
                        }
                        
                    }
                    
                    if (currentRefs.length === 0) {
                        cancel = true;
                        matches = [];
                        break;
                        
                    } else {
                        matches.push(currentRefs);
                    }
                    
                }
                
                if (cancel) break;
            }
            
            let existingGroupCandidate = undefined;
            
            if (matches.length === props.length) {
                let currentPropIndex = 0;
                for (const propMatch of matches) { // const propMatch of [ [ 'log' ] ]
                    
                    // Changes above means this will probably not be called
                    if (propMatch.length > 1) {
                        if (!existingGroupCandidate) {
                            console.log(colorText(`Multiple candidates for: ${key} based on ${props[currentPropIndex]}`, colYellow));
                            
                            for (let i = 0; i < propMatch.length; i++) {
                                const prop = propMatch[i];
                                console.log(`${i+1}: ${prop} - ${shortlist(receiverModel[prop].blocks)}`);
                            }
                            
                            const choice = await getUserInput(`Enter your choice 1-${propMatch.length} or 'new' to create new group: `);
                            const numChoice = Number(choice);
                            
                            if (isNaN(numChoice) || numChoice <= 0 || numChoice > propMatch.length) break;
                            existingGroupCandidate = propMatch[numChoice - 1];
                            
                        } else if (!propMatch.includes(existingGroupCandidate)) {
                            console.log(colorText(`Error: Some properties on ${existingGroupCandidate} are missing for ${key}. You will be asked to create a new group.`, colRed))
                            existingGroupCandidate = undefined;
                            break;
                        }
                    } else {
                        existingGroupCandidate = propMatch[0];
                    }
                    currentPropIndex++;
                }
                
            }
            if (!existingGroupCandidate) {
                let newGroupName = await getUserInput(`Enter new property group name for ${colorText(key, colGreen)}: `);
                
                while (receiverModel[newGroupName]) {
                    newGroupName = await getUserInput(colorText(`Error: ${newGroupName} already registered. Please choose a different name: `, colRed));
                }
                
                receiverModel[newGroupName] = { blocks: [ key ], properties: block.properties };
                
                for (const prop of props) {
                    if (processedProperties.has(prop)) {
                        processedProperties.get(prop)[newGroupName] = block.properties[prop];
                    } else {
                        processedProperties.set(prop, { [newGroupName]: block.properties[prop] });
                    }
                }
                console.log(`[${index}] Block ${colorText(key, colGreen)} was added to ${newGroupName}.`);
                
            } else {
                receiverModel[existingGroupCandidate].blocks.push(key);
                
                console.log(`[${index}] Block ${colorText(key, colGreen)} was added to ${existingGroupCandidate} automatically.`);
            }
            
        } else {
            singleState++;
            index++;
        }
    }

    if (singleState > 0) console.log(`${singleState} blocks had a single state and were skipped`);

    const outString = JSON.stringify(receiverModel);

    console.log('=======================================================');
    console.log('Finished processing blocks');
    console.log(`${processedProperties.size} states captured`);
    console.log('=======================================================');

    let saving = true;
    while (saving) {
        const outPath = await getUserInput('Enter output file path (e.g. output.json): ');

        try {
            await writeFile(outPath, outString);
            saving = false;
        } catch (err) {
            console.error(`Could not write to file: ${err}`);
        }
    }

    console.log('Done!');

    releaseReadline();
}

function shortlist(items, limit = 2) {
    const listing = items.slice(0, limit);
    const diff = items.length - limit;
    
    return listing.join(', ') + (diff > 0 ? ` and ${diff} more.` : '');
}

function getUserInput(prompt) {
    return new Promise((resolve) => {
        rl.question(prompt, (answer => resolve(answer)));
    });
}

function colorText(str, col) {
    return useColors ? `${col}${str}${colReset}` : str;
}

function releaseReadline() {
    rl.close();
}
