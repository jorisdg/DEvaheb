# DEvaheb
Decompiler for compiled Icarus scripts / IBI files.

## History and Original Source
Back in 1999 a demo was released for Raven Software's **[Star Trek: Voyager Elite Force](https://en.wikipedia.org/wiki/Star_Trek:_Voyager_%E2%80%93_Elite_Force)** game. The community found out the scripted sequences were in the game's data files, in the form of binary .IBI files. At the end of high school my programming skills were largely self-taught, and I hadn't entirely moved on from Turbo Pascal programming on my old DOS computer. I spent countless nights looking and finding patterns in the IBI files, and create a decompiler/compiler to make changes to scripts in the demo. Not everything worked, some things crashed, but it was a very cool feat to have accomplished.

When the real game came out, I was running efmodcentral.com - a popular Elite Force mod website (sponsored by FrontFiles.com). Raven Software gave me the SDK before most other sites, and I hosted it proudly there. Based on a bit more information thanks to playing with BehavED (the script editor) to compile scripts, I created a new version of my tool. It would only decompile (since an official compiler existed that didn't crash) and open like a text editor. The IBI reading wasn't full-proof but worked on most of the IBI scripts.

The tool was written in Delphi, with my (looking back now) limited programming skills. You can find the original source [in the orig_delphi_src](orig_delphi_src/) folder.

I forget exactly when I moved on, but I got involved in Tribes 2. Further improvements and plans for another tool were forgotten and lost.

## 1999 is calling
In 2022, a Google search related to myself revealed a search result containing "DEvaheb". Curiously, I clicked through and found out the DEvaheb tool is still being hosted on sites dedicated to Jedi Knight and other Raven Software games. The **Icarus** scripting engine from Raven Software made it to other games, and the tool mostly still worked (with all its warts from before).

## DEvaheb 2.0
With an education and 20+ years as an IT professional under my belt, I dare say my programming skills have somewhat improved. As Scott Hanselman would say: "I'm the world's okayest programmer". Kind of a 3.6: not great, not terrible.

I wrote this code in .NET 6.0 and you can find the source under[src](src/). This it's .NET6 it can be compiled for other platforms, although *for MacOS it would have to deal with endianness* which it currently doesn't do.

## Node Structure Approach
Although I've professionally worked on teams owning compilers, I'm not a compiler expert by a long shot. However, I do know the value of representing code in an intermediate tree. I wouldn't call this implementation a proper AST or any of the sorts, but it does its job.

### Visitors
The decompiler will parse the IBI file into a tree structure. It can then be reasoned over with Visitors. [One visitor generates source code](src/DEvahebLib/Visitors/GenerateIcarus.cs), mimicking the BehavED generated source as close as possible. In fact, there are [tests](src/DEvahebLibTests/) that take an IBI, generate source, and compare it to the original BehavED source files.

- Can we create a visitor to check for code that compiles but would likely (or certainly) not run in-game? YES!
- Can we create a visitor to generate IBI files, effectly turning into a compiler? YES! (we'd need to write a text file parser to create the Nnode tree first though)

## Open Source
This source is now under MIT license, for the community of modders out there. I hope you'd consider just contributing features here but if not, have fun with whatever you're planning to do with the DEvaheb source code.