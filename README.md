[![Build Status](https://travis-ci.org/StreetLevel/Unity.jl.svg?branch=master)](https://travis-ci.org/StreetLevel/Unity.jl)

[![Coverage Status](https://coveralls.io/repos/StreetLevel/Unity.jl/badge.svg?branch=master&service=github)](https://coveralls.io/github/StreetLevel/Unity.jl?branch=master)

[![codecov.io](http://codecov.io/github/StreetLevel/Unity.jl/coverage.svg?branch=master)](http://codecov.io/github/StreetLevel/Unity.jl?branch=master)

# Unity.jl 
## A scientific visualization interface for Unity

This library provides a TCP-Interface for the transmission of 3d mesh-data between Julia and Unity. 


### Installation

You need Unity to be installed, which is free for non-profit use. I got Version 2017.4.1f1 (9231f953d9d3) Personal installed on a MacBook with macOS 10.13.3.

Please run:
```Julia
Pkg.clone("https://github.com/StreetLevel/Unity.jl")
```

### Agenda

In numeric simulation we operate mostly on mesh-data, so we need easy-to-use but efficient tools for visualization for fast development cycles. The "de-facto-standard" MATLAB covers this with its patch-routine. Here Unity is used for visualization, which is a great and actively developed platform. Once you've got your data imported into Unity, you can take advantage of a huge community and tons of add-ons for high-quality custom plots. Furthermore Unity runs on various platforms, so, with little effort, you could use your tablet or even mobile phone for displaying the output.

### Usage

This package is in a very early development stage but to my knowledge it should work.

1. *Unity:* Start Unity and load the Unity Project *Unity_TCPInterface*.
2. *Unity:* Load *Scene1*
3. *Unity:* Press Play
4. *Julia:* 
```Julia
import Unity: UnityMesh, Vector3
using ColorTypes

#define vertices
verts = [Vector3(0,0,0),Vector3(1,0,0),Vector3(1,1,0),Vector3(0,1,0)]
#define indices for surface mesh
surf_inds = Int32[0,2,1,0,3,2]
#define indices for line mesh
line_inds = Int32[0,1,1,2,2,3,3,0]
#define indices for vertex mesh
vert_inds = Int32[0,1,2,3]
#define colors
colors = rand(RGBA{Float32},length(verts))
#define optons
options = ["shader = flat"]

#create meshes
surface_mesh = UnityMesh("Mesh 1", verts, Int32[], Int32[], surf_inds, colors, options)
line_mesh = UnityMesh("Mesh 2", map(x->Vector3(x.x+3,x.y,x.z),verts), Int32[], line_inds, Int32[], colors, String[])
point_mesh = UnityMesh("Mesh 3", map(x->Vector3(x.x,x.y+3,x.z),verts), vert_inds, Int32[], Int32[], colors, String[])
mesh = UnityMesh("Mesh 4", map(x->Vector3(x.x+3,x.y+3,x.z),verts), vert_inds, line_inds, surf_inds, colors, options)

#send meshes to unity
socket = connect(8052)
write(socket, surface_mesh)
write(socket, line_mesh)
write(socket, point_mesh)
write(socket, mesh)
close(socket)
```
You should see something like this:
<span style="text-align:center">![Unity Mesh](https://github.com/StreetLevel/Unity.jl/blob/master/images/meshes01.png "meshes01.png")</span>

## Different shader for surface meshes

| wireframe                        | flat                         | 
| :------------------------------: |:----------------------------:| 
| ![img_bone][img_bone_wireframe]  | ![img_bone][img_bone_flat]   | 
|smooth                            |transparent                   |
|![img_bone][img_bone_smooth]      |![img_bone][img_bone_transparent]|

[img_bone_wireframe]: https://github.com/StreetLevel/Unity.jl/blob/master/images/bone_wireframe_shader.png "wireframe shader"
[img_bone_flat]: https://github.com/StreetLevel/Unity.jl/blob/master/images/bone_flat_shader.png "flat_shader"
[img_bone_smooth]: https://github.com/StreetLevel/Unity.jl/blob/master/images/bone_smooth_shader.png "smooth shader"
[img_bone_transparent]: https://github.com/StreetLevel/Unity.jl/blob/master/images/bone_transparent_shader.png "transparent shader"

## TODO

* face colors (could be already done by duplicating the vertices for each connected face)
* auto setup of tcp connection
* simple gui for ip address etc.
* partial transmission of meshes (e.g. only update the vertices or colors)
* colorbar
* coordinate axes
* nicer transparency shader
* nicer shader for line mesh
* nicer shader and primitive for point mesh
* quad meshes
* point size

