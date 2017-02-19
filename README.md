# JAM-Unpacker
A program to unpack JAM files from The Grim Adventures of Billy and Mandy game. I used to play this game a lot as a kid, and even though it was just your average licensed fighting game, it had a special charm to it that I really liked.

I decided I wanted to figure out how the game worked, so that I could modify certain things about it. Some fighting moves are, quite frankly, too strong (I'm looking at you, Grim's mid-air heavy attack), so toning them down would be nice. Cosmetic mods might be cool too, and who knows, maybe new characters/stages will be possible at some point. Only time will tell. Yes, I'm aware this is a console game, but the console game modding scene is stronger than ever what with stuff like Brawl modding or Melee mods.

## Usage
JAMUnpacker \<jamfile.JAM\>

The tool will create a directory with the filename of your JAM with an _unpacked suffix, so LgDialog.JAM would unpack to LgDialog_unpacked.

__Note:__ The filetype table almost always has a null file extension. Files without an extension won't get an extension when unpacked, so don't be alarmed if you see a file without an extension. It is not a bug.

## Compatibility
| Platform      | Tested |
|---------------|--------|
| GameCube      | Yes    |
| Wii           | No     |
| PlayStation 2 | No     |

## For the future
I want to add repacking. The file structure of JAM files is pretty simple, so I should be able to create a packer that can make JAM files from scratch. Keep your eyes peeled on my GitHub (it will be a separate tool) for a packer, once this has a 1.0 release.

I also want to create tools to modify files that reside inside the JAM files. There are a decent amount of plaintext files, so those can be edited with any text editor, but of course there will be model geometry and such, which isn't plaintext. Textures (at least on GameCube) seem to be TPL and TGA, which already have plenty of support for editing by other tools already, so I won't make tools for those. Maybe if the Xbox/PS2 texture formats are different, which they almost certainly will be considering TPL is a Nintendo format.

I'm hoping models and such are kept as simple as JAM files - I'd really love to be able to modify models. Maybe with enough hacking we could see entirely new characters and stages added to the game.
