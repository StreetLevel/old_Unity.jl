import Unity: UnityMesh, PyramidMesh, UnityVector3, UnityText
using ColorTypes

#define vertices
verts = [UnityVector3(0,0,0),UnityVector3(1,0,0),UnityVector3(1,1,0),UnityVector3(0,1,0)]
#define indices for surface mesh
surf_inds = Int32[0,2,1,0,3,2]
#define indices for line mesh
line_inds = Int32[0,1,1,2,2,3,3,0]
#define indices for vertex mesh
vert_inds = Int32[0,1,2,3]
#define colors
colors = rand(RGBA{Float32},length(verts))
#colors = [RGBA{Float32}(1,1,1,.001) for vert in verts]
#define optons
options = ["surface_shader = flat"]
point_options = ["point_size = 0.05"]

#text
unity_text = UnityText("Hello World!", UnityVector3(0,1,0), UnityVector3(0.15,0.15,.15), UnityVector3(1,1,1))

#create meshes
surface_mesh = UnityMesh("Mesh 1", verts, Int32[], Int32[], surf_inds, colors, options, UnityText[unity_text])
line_mesh = UnityMesh("Mesh 2", map(x->UnityVector3(x.x+3,x.y,x.z),verts), Int32[], line_inds, Int32[], colors, String[])
point_mesh = UnityMesh("Mesh 3", map(x->UnityVector3(x.x,x.y+3,x.z),verts), vert_inds, Int32[], Int32[], colors, point_options)
mesh = UnityMesh("Mesh 4", map(x->UnityVector3(x.x+3,x.y+3,x.z),verts), vert_inds, line_inds, surf_inds, colors, vcat(point_options,options))

#send meshes to unity
socket = connect(8052)
write(socket, surface_mesh)
write(socket, line_mesh)
write(socket, point_mesh)
write(socket, mesh)
close(socket)