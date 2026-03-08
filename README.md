# DEvaheb
Decompiler for compiled Icarus scripts / IBI files.

Quick Links:
* **[Download](https://github.com/jorisdg/DEvaheb/releases)** the latest version
* **[Using](./Usage.md)** DEvaheb
* **[History](./History.md)** of the old tool and its revival

## History and Original Source
For a little history on how this tool came to be, and the re-discovery of the old DEvaheb.exe floating on this internet, [go here](./History.md).

## DEvaheb 2.0
I wrote this new code in C# with .NET 9.0 and you can find the source under [src](src/). Since it's .NET10 it can be compiled for other platforms, although *for MacOS it would have to deal with endianness* which it currently doesn't do.

DEvaheb 2 does not have a UI for editing files, but the command line tool can automatically open an editor of your choosing. See [using DEvaheb](./Usage.md) for more information and some tips if you're not comfortable using command line.

## suraci 1.0
Immediately after remaking DEVaheb, questions were asked if the tool could also compile back to IBI. This would provide an opportunity to add in validations and avoid common pitfalls with icarus scripts. 

Because of BehavED, a new executable made sense as a drop-in replacement for IBIze.exe with the same command line parameters (as opposed to adding compile parameters to DEvaheb). See [using suraci](./Usage.md) for more information.

### Library
The command line tools DEvaheb.exe and suraci.exe are largely a shell over the class library DEvahebLib, which contains all of the logic. The idea is if anyone feels inclined to build other tools for scripting, they can re-use my library's logic.

## Node Structure Approach
Although I've professionally worked on teams owning compilers, I'm not a compiler expert by a long shot. However, I do know the value of representing code in an intermediate tree. I wouldn't call this implementation a proper AST or any of the sorts, but it does the job.

### Visitors
The decompiler will parse the IBI file into a tree structure. It can then be reasoned over with Visitors. [One visitor generates source code](./src/DEvahebLib/Visitors/GenerateIcarus.cs), mimicking the BehavED generated source as close as possible. In fact, there are [tests](./src/DEvahebLibTests/) that take an IBI, generate source, and compare it to the original BehavED source files.

The visitors are now used to implement type checkers and other code checks, as well as the IBI compiler.

## Games
From what I can find, Icarus scripting is used in the following Raven Software games:
- [Star Trek: Voyager Elite Force (2000)](https://en.wikipedia.org/wiki/Star_Trek:_Voyager_%E2%80%93_Elite_Force) : Icarus v1.32
- [Soldier Of Fortune 2: Double Helix (2002)](https://en.wikipedia.org/wiki/Soldier_of_Fortune_II:_Double_Helix) : Icarus v1.32
- [Star Wars Jedi Knight 2: Jedi Outcast (2002)](https://en.wikipedia.org/wiki/Star_Wars_Jedi_Knight_II%3A_Jedi_Outcast) : Icarus v1.33
- [Star Wars Jedi Knight: Jedi Academy (2003)](https://en.wikipedia.org/wiki/Star_Wars_Jedi_Knight:_Jedi_Academy) : Icarus v1.33
- [X-Men Origins: Wolverine (2009)](https://en.wikipedia.org/wiki/X-Men_Origins:_Wolverine_(video_game)) : (Icarus version TBD)

Other than X-Men Origins, these games can still be purchased at [https://www.gog.com/](https://www.gog.com/). You can find copies of X-Men Origins for PC on EBay.

## Community
There may be other modding communities using DEvaheb, but the active one I got in touch with is related to [JKHub.org](https://jkhub.org/) and their [Discord server](https://discord.me/jediknight). You can find me there under my user name "Interface".

## Related Links
- https://github.com/JACoders/OpenJK/wiki/ICARUS-Scripting
- https://github.com/JACoders/OpenJK/tree/master/code/icarus


## Open Source
This source is now [under MIT license](./LICENSE), for the community of modders out there. I hope you'll consider just contributing features here but if not, have fun with whatever you're planning to do with the DEvaheb source code.
