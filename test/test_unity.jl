import Unity: UnityMesh, PyramidMesh, Vector3
using ColorTypes

verts = [Vector3(0,0,0),Vector3(1,0,0),Vector3(1,1,0),Vector3(0,1,0)]
surf_inds = Int32[0,2,1,0,3,2]
line_inds = Int32[0,1,1,2,2,3,3,0]
vert_inds = Int32[0,1,2,3]
color = rand(RGBA{Float32},length(verts))
#color = [RGBA{Float32}(1,1,1,1) for vert in verts]

options = ["surface_shader = flat"]

surface_mesh = UnityMesh("Mesh 1", verts, Int32[], Int32[], surf_inds, color, options)
line_mesh = UnityMesh("Mesh 2", map(x->Vector3(x.x+3,x.y,x.z),verts), Int32[], line_inds, Int32[], color, String[])
point_mesh = UnityMesh("Mesh 3", map(x->Vector3(x.x,x.y+3,x.z),verts), vert_inds, Int32[], Int32[], color, String[])
mesh = UnityMesh("Mesh 4", map(x->Vector3(x.x+3,x.y+3,x.z),verts), vert_inds, line_inds, surf_inds, color, options)

socket = connect(8052)
write(socket, surface_mesh)
write(socket, line_mesh)
write(socket, point_mesh)
write(socket, mesh)
close(socket)