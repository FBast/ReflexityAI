# When to use an Utility AI ?

While utility-based systems can be used in many types of games, they are more appropriate in situations where there are a large number of potentially competing actions the AI can take—often with no obvious “right answer.” In those times, the mathematical approach that utility-based systems employ is necessary to ferret out what the most reasonable action to take is. Aside from The Sims, other common areas where utility-based systems are appropriate are in RPGs, RTS, and simulations.

Source : http://intrinsicalgorithm.com/IAonAI/2012/11/ai-architectures-a-culinary-guide-gdmag-article/

# How to start using xNodeUtilityAI

- Firstly, download the last release and import it into your Unity Project.
- Create a script **MyAIBrain** inheriting from AbstractAIBrain.
- Create a script **MyAIComponent** inheriting from **AbstractAIComponent** and add it to your GameObject.
- Create scripts inheriting from **SimpleActionNode**, **SimpleEntryNode** or **CollectionEntryNode**.
- Create a **MyAIBrain** using Unity ScriptableObject contextual menu.
- Add your custom nodes on the Node graph with **UtilityFunctionNode** and **OptionNode**.
- Check the wiki for in depth informations !
