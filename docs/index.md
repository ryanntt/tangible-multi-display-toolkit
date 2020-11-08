# Tangible Multi-Display Toolkit

*This documentation is in progress.*

Purpose of project. And link to conference.

There are 3 main components of project:
- View: Top down
- View: First person view
- Participant controller

## Installation

### Views

Steps to install the application on 3 iPads

- Clone this repo

```markdown
git clone https://github.com/ryanntt/tangible-multi-display-toolkit.git
```

- Initialize the Unity project

As we all used standard packages for Unity, once you import the project, the initialization will download required assets from Unity Store. The assets not available on Unity Store are kept to small sizes in the repository.

The project work well on version 2019.1.0f2. It should work with the newer versions of Unity and Xcode with some adjustment. 

- Configure the IP address of controller.

In this repository for Camera view, we are using [Photon Realtime](https://www.photonengine.com/en-US/Realtime) to sync the position of care between all devices. This project is currently using my project's quota and it is limited to 20 players at the same time. Please feel free to use it or setup your own for more quota.

As the controller iPad needs to have fixed IP to communicate with the view iPads, we will need to have our own local network. We used [device_name](#) in our study but any other device should do the job.

In `Assets/Scripts/UDPReceive.cs` line 27:
```
    public string IP = "192.168.0.107";
```

Use your own IP number here



### Controller

- Clone the repo:
```
git clone https://github.com/HoggenMari/AVLightingToolkit.git
```

## Setup and running

How to setup 3 iPads

## Configure the toolkit

### LED Colours
We use HEX value to display the color. This might be difficult for lighting designer to manipulate the colors. You will need a converter to get HEX from HSL or RGB. There are many web tools available online for this. We used [ColorSlurp](https://colorslurp.com/), an free Mac app.


To change colour of LED in Script, run public function ```ChangeColor()``` using following syntax:
```ChangeColor(string[] string )```

For example:

```ChangeColor(new string[] { "#00FF00", "#FF0000", "#0000FF", "#00FF00", "#FF0000", "#0000FF" });```

The function will change colours of all 21 LEDs. If the number of colours is less than 21, then random colours will be assigned. This function is declared in file ```Scripts/LEDControl.cs```.

### Context

To change the text of any indicator, target ``Context Manager script`` in ``object Context Manager`` and run public function:
`UpdateContext(int i, string text)` to update the text of context **i**th. **i** ranges from 1 to 4.


## Deployment and Setup

Deploy the project to iPad

In `Project` panel, go to `Assets/Scenes`, you should see 2 unity files:
- CityCar-Scene
- Park-Scene

They are examples that you can run with, and will be good reference for how to setup environment and interactive elements.

Go to menu `Files/ Build Settings`
- Choose the scene you want to build
- Select Platform: default is PC, Mac. Please select iOS and install required package to build the application for iOS.

Once you have Unity for iOS package downloaded and installed, you can build this application for iPad. 


## Troubleshooting
Some common problem and how to fix it.

- Crashed when starting the application: 

## Contributing

Having trouble with Pages? Check out our [documentation](https://docs.github.com/categories/github-pages-basics/) or [contact support](https://github.com/contact) and we’ll help you sort it out.