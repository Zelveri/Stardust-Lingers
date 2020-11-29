# Stardust Lingers command reference:

### Explanation:
Parameters in `<>` are required!  

Parameters in `[]` are optional
- `[xxx]` means you can write the word xxx,   
- `[xxx=3]` means you can write a number, if not, the given default value is used
---
## Commands
Sorted by providing GameObjects:  

### YarnCommands.cs/.prefab:

#### `<< transition <type> [speed=1] >>` {#transition}
Executes a transition  

* **type**: string - Transition type:  
  - `None`: no transition, background is directly changed, see backdrop command  
  - `Fade`: new background fades in, see backdrop command  
  - `Fade_In`: fade in from black  
  - `Fade_Out`: fade out to black  
  - `Crossfade`/`Cross_Fade`: fade to black, change background then fade in again, see backdrop command
  - `Slide`: Slide black rect over screen, change background, slide black rect away, see backdrop command

* **speed**: float - Speed multiplier for the animation, default = 1 

Examples:  

    << transition None >>
    << transition Slide 1.5 >>

#### `<< backdrop <filename> >>`
Select new background sprite, change happens when << transition >> is called the next time
- **filename**: string - name of a sprite in "Artwork/Backgrounds/"  

Example:  
    
    << backdrop Livingroom_Night >>
    << transition Cross_Fade>>

#### `<< nametag <name> [hidden] >>`
Change the nametag and the dialogue background for a new character  
- **name**: string - the new name:  
    - one of the names registered in the DialogueAnimator.cs nameToTextureDict: Mira, Lune, Trevis
- **hidden**: if nametag should display "???", add `hidden` at the end  

Examples:

    << nametag Mira >>
    << nametag Lune hidden >>


#### `<< hide_dialogue >>`
Hide the dialogue box and nametag.

#### `<< show_dialogue >>`
Show dialogue box and nametag.

#### `<< sprite <charactername> <spritename> [animation=Bounce] >>`
Show a character sprite with a little animation.  
- **charactername**: string
	- `None`: sprite will disappear during next transition
	- else: load sprite `"Artwork/Character/<charactername>/<spritename>"`
- **spritename**: string  

- **animation**: string
    triggername from the Character animationcontroller to play, default = `Bounce`

Examples:

    << sprite Bunny Open_Open >>
    << sprite Bunny Open_Blep Fade_Out >>

#### `<< schedule_transition >>`
Is called on every node start, causes different behaviour for the following commands:  
    `nametag`, `sprite`, `hide_dialogue`  
    these commands will not do an immediate change, but instead wait until the next transition command causes the screen to go black
    so that changes can happen invisible

Example:  

    << schedule_transition >>
    << nametag Mira >>
    << transition Slide >>

---

### ItemCanvas/ItemBehaviour.cs:

#### `<< item <itemname> >>`
Display a sprite on the item canvas, fades in and out
- **itemname**: string - the name of the item to display
    - `None`: item disappears
    - else: display sprite `"Artwork/Items/<itemname>"`

Examples:

    << item Note >>
    << item None >>

---

### Phone/PhoneDialogue - LineUpdateHandler.cs
**DO NOT USE THIS AND YarnCommands AT THE SAME TIME!!**

#### `<< nametag <name> >>`
For phone dialogue, selects the side the message bubbles appear.
- **name**: string
    - `Mira`: bubbles are to the right and green
    - else: bubbles are to the left and blue

#### `<< photo <imagename> >>`
Display an image as phone message
- **imagename**: string  
    loads the image `"Artwork/Photos/<imagename>"`

#### `<< transition <type> [speed=1] >>`
same as [YarnCommands transition](#transition)

---

### GameManager/PersistentCommands.cs provides:
**DO NOT USE OUTSIDE GAMEMANAGER**

#### `<< music <action> <filename> [duration=3] >>`
Plays music
- **action**: string
    - `Play`: plays the sound file `"Music/<filename>"` on loop
    - `Stop`: stop the music, does not need additional arguments
    - `Fade_In`: plays the sound file `"Music/<filename>"` in a loop after fading it in
    - `Fade_Out`: fades the current music out and stops it
- **filename**: string  
the filename of the sound file to load, not needed for `Stop` and `Fade_Out`
- **duration**: float  
duration of the fade in seconds, default is 3 seconds

Example:

    << music Play lune_theme >>
    << music Fade_In lune_theme 4.2 >>
    << music Fade_In lune_theme >>
    << music Fade_Out 4.2 >>
    << music Fade_Out >>
    << music Stop >>

#### `<< sound <action> <filename> [loop] [fade] >>`
Plays a sound effect
- **action**: string
    - `Play`: play the file 
    - `Stop`: stop the effect, only possible if looping
- **filename**: string  
    name of the sound file to load, `"Sounds/<filename>"`
- **loop**: add this to make the sound loop until stopped, or scene changed
- **fade**: add this to make the sound fade in or out with 2 sec fade duration

Example:

    << sound play wind >>
    << sound play wind loop >>
    << sound play wind loop fade >>
    << sound play wind fade >>
    << sound stop wind >>
    << sound stop wind fade >>

#### `<< scene <scenename> >>`
Change to new scene
- **scenename**: string  
    the name of the scene to change to  
    **!! has to be registered in the Build Settings and added to the `Scenes` enum in SceneController.cs, 
    the enum values need to have the same order as in the build settings !!**
