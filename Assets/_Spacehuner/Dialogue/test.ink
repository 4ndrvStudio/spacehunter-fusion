-> main

=== main ===
Which pokemon do you choose?
    + [Charmander]
        -> chosen("Charmander")
    + [Bulbasaur]
        -> chosen("Bulbasaur")
    + [Squirtle]
        -> chosen("Squirtle")
        
=== chosen(pokemon) ===
Which a color do you choose? 
    + [Yellow]
        ->chosenColor("Yellow")
    + [Red]
        ->chosenColor("Red")
    + [Blue]
        ->chosenColor("Blue")
    
        
=== chosenColor(color) === 
You chose {color}!
-> END