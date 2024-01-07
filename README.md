# ParticleSystems

1/6 simple waterfountain / explosion particle system. Just has initial velocity and gravity.

1/6 Refactored code to use the emitter model and make it easier to see what parts of the particle system are controller by the user (ie. initial parameters and updating particles as the simulation goes on).
The idea for this particle system is that a user should be able to make a script that will dynamically alter the particles with time. Then I'll have a library of these scripts and you
can just attach them to the emitter.

1/6 Changing the opacity as time goes on and using a color gradient can get some cool fire/explosion kind of effects. If you alternate the initial
radii from small to large you get these cool explosion effects. The particles with large radii look like the initial explosion
and the small radii looks like debri or embers flying off.

References:
https://dl.acm.org/doi/pdf/10.1145/280811.280996
https://www.sfml-dev.org/tutorials/2.0/graphics-vertex-array.php
