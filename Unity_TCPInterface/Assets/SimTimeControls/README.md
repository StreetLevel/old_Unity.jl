
Simulation Time Controls
========================

SimTimeControls.unitypackage contains a prefab of a UI Panel for representing
and controlling time. The time control functionality is primarily intended to
aid in the visualization of biophysical processes (e.g. a hydrology model of a
watershed). Such biophysical models are discrete time models with "simulation
steps" at equal intervals throughout the duration of the model.

A TimeSlider component is attached to a Unity 4.6+ UI Slider gameobject. The 
TimeSlider component provides a public interface for controlling whether or not
the slider is playing (running through time) and for setting the playback speed
relative to wall time. The TimeSlider component also exposes public properties 
conveying the current simulation step and simulation time.

Using the SimTimeControls Prefab
--------------------------------
The prefab requires Unity 4.6 or higher. Create a UI Canvas and place the 
SimTimeControls prefab as a child of the Canvas.

In the Inspector view the start date can be specified by selecting the Time 
Slider object in the Hierarchy panel. In the "Start Time Str(ing)" field enter 
the date at which the timer should begin. In the "Time Delta Hours" field 
specify the time between simulation steps in hours. The Total number of steps 
can be specified using the "Max Value" property under the default Slider 
component. The prefab is configured to represent one 365 day year with one hour
between time steps.

Playspeed Slider Control
------------------------
The slider to the right specifies the relative speed between walltime and 
simulation time. The Speed Selection Mode can be specified as Linear or 
Exponential. 

Exponential
+++++++++++
When it is exponential you have an inspector field to specify the baseline play 
speed rate in hours / second (the default is 1). The play speed slider will move 
in halvings and doublings of this baseline rate providing 7 discrete playback 
speeds. 

Linear
++++++
Inspector fields will allow you to specify the minimum and maximum rates (The 
minimum default rate is realtime (1 wall second : 1 simulation second). When in 
the linear mode the slider moves continuous rather than having 7 discrete 
speeds.

Accessing TimeSlider Properties During Runtime
----------------------------------------------
Rather than representing simulation in multiple locations in a project, the
SimTimeControls prefab can keep track of time for all objects in the project.

This can be accomplished by having other components (MonoBehaviors) keep a
reference to the TimeSlider component. Please refer to the 
UpdateUISimStepText.cs or UpdateUISimDateText for examples on how you can find
the TimeSlider component and access the SimStep and SimTime controls

Public Properties
+++++++++++++++++

IsPlaying (bool)
    Specifies whether the TimeSlider is progressing or paused
 
PlaySpeed (float)
    Specifies amount of simulation time in hours that elapses compared to wall
    time in seconds. 
        PlaySpeed = 1 -> 1h of sim time  : 1 s of wall time

StartTime (DateTime)
    Read only, the date and time at step 0
    
DeltaTime (DateTime)
    Read only, time interval between steps
    
SimStep (Int64)
    The current simulation step calculated as the floor of the slider's value
    
Caveats
-------
The StartTime and DeltaTime properties are set at runtime and cannot be updated 
dynamically

License
-------
This software is licensed under the BSD 3-Clause License

Copyright (c) 2015, Roger Lew (rogerlew@gmail.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this 
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, 
   this list of conditions and the following disclaimer in the documentation 
   and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors 
   may be used to endorse or promote products derived from this software without 
   specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR 
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON 
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (
INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


NWbtnPackage
------------

NWbtnPackage is the Nicholas Woods Button Package and is licensed under 
CreativeCommons Attribution-ShareAlike (CC BY-SA)