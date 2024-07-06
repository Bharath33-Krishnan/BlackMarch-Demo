# ProgrammingAssignmentBlackMarch
Programming Assignment for Black March Game Studio\
Features Includes:
- 10x10 Grid generation using data from a scriptable object
- Editor Script to edit the scriptable object and save it
- Custom Inspector for the editor script
- Custom path finding implementation
- a few custom lit shaders 

# Unity Version
Project Uses Unity Version 2022.3 LTS\
The project uses Universal Render Pipeline\
Use the Assignment scene in scenes folder

# Known Bugs
- Rarely enemies might clump up
- Player may phase through enemies sometimes
- Enemies will stop path finding if player is locked in place\
  \
Note: Since the path finding algorithm can't make diagonal moves, \
 player will be stuck if surrounded in all 4 directions by enemies or obstacles

