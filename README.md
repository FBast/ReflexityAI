# How to start using xNodeUtilityAI

- Firstly, download the last release and import it into your Unity Project.
- Create a script **MyAIBrain** inheriting from AbstractAIBrain.
- Create a script **MyAIComponent** inheriting from **AbstractAIComponent** and add it to your GameObject.
- Create scripts inheriting from **SimpleActionNode**, **SimpleEntryNode** or **CollectionEntryNode**.
- Create a **MyAIBrain** using Unity ScriptableObject contextual menu.
- Add your custom nodes on the Node graph with **UtilityFunctionNode** and **OptionNode**.
- Check the wiki for in depth informations !
