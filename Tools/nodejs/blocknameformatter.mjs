import { access, readFile, writeFile } from 'fs/promises';
import { constants } from 'fs';

const args = process.argv.slice(2);

if (args.length < 1) {
    console.info('Usage: node blocknameformatter.js <blocks.json file>');
} else {
        begin(args.join(''));
}

async function begin(path) {
    try {
        await access(path, constants.R_OK);
        
    } catch  {
        console.error(`File not found: ${path}`);
        return;
    }
    
    
    let blocksJson;
    
    try {
        blocksJson = JSON.parse(await readFile(path, 'utf-8'));
        
    } catch (err) {
        console.error(err);
        return;
    }
    const keys = Object.keys(blocksJson);

    const outputMap = {};

    console.log(`Loaded ${keys.length} namespaced blocks`);
    for (const key of keys) {
        
        if (key.startsWith('minecraft:')) {
            const nameParts = key.slice(10).split('_');
            const cName = nameParts.map(x => x.charAt(0).toUpperCase() + x.slice(1)).join('');
            outputMap[key] = {
                cName,
                states: blocksJson[key].states.length
            };
            
        } else {
            console.error('Blocks outside "minecraft:" namespace not supported');
            
        }
    }
    
    await writeFile('formattedblocks.json', JSON.stringify({blocks: outputMap}));
    
    console.log('Done!');
}
