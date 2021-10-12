import { access, readFile, writeFile } from 'fs/promises';
import { constants } from 'fs';

const args = process.argv.slice(2);

if (args.length < 1) {
    console.info('Usage: node chunk2bin.js [paletteSize] <raw file>');
    console.info('Set paletteSize to 0 to disable palette splitting');
} else {
    const paletteSize = Number(args[0]);
    
    if (isNaN(paletteSize) || paletteSize < 1) {
        begin(args.join(''));
    } else {
        begin(args.slice(1).join(''), paletteSize);
    }
    
}

async function begin(path, paletteSize = -1) {
    try {
        await access(path, constants.R_OK);
        
    } catch  {
        console.error(`File not found: ${path}`);
        return;
    }
    
    try {
        const buf = await readFile(path);
        
        let binStr = '';
        buf.forEach((x, i) => {
            binStr += pad(x.toString(2), '0', 8) + ((i + 1) % 8 === 0 ? '\n' : ' ');
        });
        
        let binStrReversed;
        
        if (paletteSize > -1) {
            const paletteBitLen = Math.max(4, Math.ceil(Math.log2(paletteSize)));
            binStrReversed = binStr.split('\n').map(x => {
                return stringChunker(x.split(' ').reverse().join(''), paletteBitLen).join(' ');
            }).join('\n');
            
        } else {
            binStrReversed = binStr.split('\n').map(x => {
                return x.split(' ').reverse().join(' ');
            }).join('\n');
            
        }
    
        await writeFile(`${path}.bin`, binStrReversed);
        console.log('Done!');
        
    } catch (err) {
        console.error(`Error while handling file: ${err}`);
    }
}

function pad(str, pad, len) {
    return str.length < len ? pad.repeat(len - str.length) + str : str;
}

function stringChunker(str, size) {
    const chunkCount = Math.ceil(str.length / size);
    const chunks = new Array(chunkCount);
    
    let firstChunk = str.length % size;
    firstChunk = firstChunk === 0 ? size : firstChunk;
    
    chunks[0] = str.slice(0, firstChunk);
    
    for (let i = 1, o = firstChunk; i < chunkCount; i++, o += size) {
        chunks[i] = str.slice(o, o + size);
    }
    
    return chunks;
}
