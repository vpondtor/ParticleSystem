# ParticleSystems

1/6 simple waterfountain / explosion particle system. Just has initial velocity and gravity.

1/6 Refactored code to use the emitter model and make it easier to see what parts of the particle system are controller by the user (ie. initial parameters and updating particles as the simulation goes on).
The idea for this particle system is that a user should be able to make a script that will dynamically alter the particles with time. Then I'll have a library of these scripts and you
can just attach them to the emitter.
