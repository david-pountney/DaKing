/* Copyright (C) 2014 Calvin Sauer - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Calvin Sauer <calvin.j.sauer@gmail.com>, May 2014
 * Feel free to email me with questions or feature requests!
 */
 
 ======================
 DragRigidBody2D v0.3 README
 ======================

To get started, drag the DragRigidBody2D script onto your main camera. 

 
 Important parameters:
 
	- Draggable Layers
		This layer mask denotes which layers are affected by dragging. An object's layer MUST be in this mask if you want it to be draggable.
		
	- Drag Damping
		This float affects how fast you would like the object to follow your mouse/touch. A higher damping makes for sluggish dragging, while a lower damping makes for snappy dragging
		Can NOT be 0!
		
	- Freeze Rotation
		Should the dragged object's rotation be frozen while it is being dragged?
		
	- Snap to Center
		If this is true, the dragged object will be picked up by its midpoint.
		Otherwise, it will be picked up by the initial contact point.
		
	- Snap Speed
		If Snap to Center is set to true, this float determines how quickly the object should be snapped
		
	- Relative to Rigidbody
		A Rigidbody2D that the dragged object will be dragged relative to. Please see the RelativeToDemo scene for an exmaple. 


