# FutScript

FutScript is a program that makes it very easy to write simple and effective mouse and keyboard macros for Windows. FutScript implements its own scripting language to interpret these macros. Macro scripts can loop repeatedly or be triggered by a hotkey. A big focus of FutScript is to make the simulated mouse and keyboard input as realistic and human-like as possible. 

## Mouse

There are several different built-in algorithms for moving the mouse cursor to a destination including teleporting directly and using a smooth linear path. The user also has the option of recording their own mouse movements and replaying those instead of using an algorithm-generated path. The user can move the mouse cursor to a specified pixel (ex: `move(100, 200)`) or move it relative to the initial cursor position (ex: `movefrom(0, 200)` to move cursor down 200 pixels). A mouse polling rate can be simluated, and the user can specify a custom polling rate. The user can also configure the mouse movement speed. 

## Keyboard

The user can type text (ex: `type(hello)`) or individually press and release keys with the key's key code/name (ex: `keydown(0x25)` and `keyup(F6)`). A keyboard polling rate can be simluated, and the user can specify a custom polling rate. The user can also configure the typing speed. 

## Randomness

Randomness also plays a large part in making the simualted input as human-like as possible. The user is able to express randomly-generated numbers very easily in FutScript. For example, the function call `wait(100~20)` means that normal distribution will be used with a mean of 100 and standard deviation of 20, and the script will pause for the resulting number of milliseconds. Evenly distributed random numbers can also be expressed by using a colon to specify an inclusive range, such as `move(80:120, 500:600)` for moving the mouse cursor to a random location within a specified rectangle. 

## Color detection

FutScript is also capable of color detection. Colors are identified in the FutScript language in terms of "color rules." This allows the user to express color tolerances and NOT operations. For example, 
* `e81123` means only colors with exactly the same RGB values is considered a match
* `e81123~10` means that any color in which all of its RGB values are within 10 of the corresponding RGB values of the specified color is considered a match
* `!e81123~10` is the same as above, but it is inverted

Built-in functions exist in FutScript to:
* check the color of a pixel and execute certain code if it matches a provided color rule
* wait for a pixel to match a certain provided color rule
* wait for a certain number of pixels within an area to change (based on a tolerance value)
* check if any pixel matching a provided color rule exists within a screen area

## Concessions

FutScript language itself is probably not the best tool for making complicated macros with a lot of control structures or variables. Its main appeal is its simple syntax and its user interface for making it quick and easy to write and generate code. Since the library code is in a dll decoupled from the form, the library code can be used directly in another application without the limitations of the FutScript language. 
