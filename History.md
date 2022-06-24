# DEvaheb History

## History and Original Source
Back in 1999 a demo was released for Raven Software's **[Star Trek: Voyager Elite Force](https://en.wikipedia.org/wiki/Star_Trek:_Voyager_%E2%80%93_Elite_Force)** game. The community found out the scripted sequences were in the game's data files, in the form of binary .IBI files. At the end of high school my programming skills were largely self-taught, and I hadn't entirely moved on from Turbo Pascal programming on my old DOS computer. I spent countless nights looking and finding patterns in the IBI files, and created a decompiler/compiler to make changes to scripts in the demo. Not everything worked, some things crashed, but it was a very cool feat to have accomplished.

When the full (Elite Force) game came out, I was running efmodcentral.com - a popular Elite Force mod website (sponsored by FrontFiles.com). Raven Software gave me the SDK before most other sites, and I hosted it proudly there. Based on a bit more information thanks to playing with BehavED (the script editor) to compile scripts, I created a new version of my tool. It would only decompile (since an official compiler existed that didn't crash) and open like a text editor. The IBI reading wasn't full-proof but worked on most of the IBI scripts.

The tool was written in Delphi, with my (looking back now) limited programming skills. You can find the original source [in the orig_delphi_src](./orig_delphi_src/) folder.

I forget exactly when I moved on, but I got involved in Tribes 2. Further improvements and plans for another tool were forgotten and lost.

## 1999 is calling
In 2022, a Google search related to myself revealed a search result containing "DEvaheb". Curiously, I clicked through and found out the DEvaheb tool is still being hosted on sites dedicated to Jedi Knight and other Raven Software games. The **Icarus** scripting engine from Raven Software made it to other games, and the tool mostly still worked (with all its warts from before).

## DEvaheb 2.0
With an education and 20+ years as an IT professional under my belt, I dare say my programming skills have somewhat improved. As Scott Hanselman would say: "I'm the world's okayest programmer". Kind of a 3.6: not great, not terrible.

Reviewing the old code I realized I had discovered more about the file type than I realized at the time. I was able to make the parser 99% reliable with what I learned from my own code. The JK (Jedi Knight) community pointed out the game engines for JK is open source, and I was able to find the code that reads and executes the Icarus scripts. That gave me the final explanation for a few things I wasn't sure about.
