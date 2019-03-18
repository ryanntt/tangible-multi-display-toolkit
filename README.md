#Urban Autonomous Car Unity Prototype - Documentation


**LED Colours**

To change colour of LED in Script, run public function ```ChangeColor()``` using following syntax:
```ChangeColor(string[] string )```

For example:

```ChangeColor(new string[] { "#00FF00", "#FF0000", "#0000FF", "#00FF00", "#FF0000", "#0000FF" });```

The function will change colours of all 21 LEDs. If the number of colours is less than 21, then random colours will be assigned. This function is declared in file ```Scripts/LEDControl.cs```.

**Context**

To change the text of any indicator, target ``Context Manager script`` in ``object Context Manager`` and run public function:
`UpdateContext(int i, string text)` to update the text of context **i**th. **i** ranges from 1 to 4.