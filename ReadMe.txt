╔═╦═╦═╗
╚═╬═╬═╝
  ╚═╝etris for C#

Hi! Welcome to this Tetris project in C#

Quick Note! The game is finished!!! Still, there is room for improvement or to make different versions of the layers, implementing other rotations like Sega-type, etc.

You can play the Core Release from here: https://drive.google.com/file/d/0B0EhlrdkYNutRzJmSFNrc3c3SVU/view?usp=sharing (google drive link)

1) Aim

The aim of this project is to show how a basic game engine works and how can you divide it's behaviour in different layers.

Although this pertains to game engines, most of the data processing, data visualizing and layering can be extrapolated to other schemas like MVC.

2) Strategy

In the most basic scenario, a game is a serie of decisions which affect the game status. 
Although this can vary greatly, for the sake of simplicity we are gonna do this tetris in a discrete, step-based game.
Finally, we have to show the information to the user in a correct way so he can choose their next actions.


      INPUT      ---\      GAME LOGIC   ---\    GAME STATUS   ---\    "RENDERING"     ---\       OUTPUT
    (decision)   ---/      (processing) ---/     (result)     ---/    (translation)   ---/   (presentation)

*I use the term rendering because is just like when a PC translates 3D Models (mostly points and vectors) to real images in pixels and colors.
 It's just the translation from a machine state to something understandable for the user

These will be the layers we will aim to implement.

3) Evaluation

The next points are used to evaluate the design decisions during development

 - Program must work.
 - Each layer must be interchangeable. This means that they should suffice as components for other games with the same requeriments.
 - Try to avoid overengineering.
 - Game must be discrete, single thread.
 - Any good practices in OOP (like SOLID design)

4) Contact info

joseraezrodriguez@gmail.com
joseraez.com
@joshiraez


Feel free to branch and contribute with anything :), ideas are: different controllers, different renders, an UI interface, or just improving the code.
