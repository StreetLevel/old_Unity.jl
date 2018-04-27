module Unity
import JSON
import ColorTypes
import Base.TCPServer
import GeometryTypes

type Vector3
    x::Float32
    y::Float32
    z::Float32
end

type CLR
    r::Float32
    g::Float32
    b::Float32
    a::Float32
end

function Base.convert(::Type{CLR},c::ColorTypes.RGBA{Float32})
    return CLR(c.r,c.g,c.b,c.alpha)
end

#Unity mesh with c-like indexing
type UnityMesh
    id::String
    vertices::Vector{Vector3}
    points::Vector{Int32}
    lines::Vector{Int32}
    triangles::Vector{Int32}
    colors::Vector{CLR}
    options::Vector{String}
end

function Base.write(socket::TCPSocket, um::UnityMesh)
    jum = JSON.json(um)
    retval = write(socket, jum*"UNITY_MESH_JSON_FORMATTED")
    sleep(.1)
    return retval
end

#Unity Pyramid mesh with c-like indices
type PyramidMesh
    id::String
    vertices::Vector{Vector3}
    pyramids::Vector{Int32}
    colors::Vector{ColorTypes.RGBA{Float32}}
end

begin
local const pattern = [ 0,2,1,0,3,2,2,3,1,0,1,3]
function Base.convert(::Type{UnityMesh},msh::PyramidMesh,dublic_vert::Bool=false)
    if dublic_vert
        return convert_and_duplicate(UnityMesh,msh,pattern)
    else
        return convert(UnityMesh,msh,pattern)
    end    
end
end

function Base.convert(::Type{UnityMesh},msh::PyramidMesh,pattern::Vector{Int})
    triangles = Vector{Int32}()
    inds = msh.pyramids
    @assert mod(length(inds),4) == 0
    for i = 1:4:length(inds)
        append!(triangles,
            [
            inds[i+pattern[1]],inds[i+pattern[2]],inds[i+pattern[3]],
            inds[i+pattern[4]],inds[i+pattern[5]],inds[i+pattern[6]],
            inds[i+pattern[7]],inds[i+pattern[8]],inds[i+pattern[9]],
            inds[i+pattern[10]],inds[i+pattern[11]],inds[i+pattern[12]]
            ]
            )
    end
    return UnityMesh(msh.id,msh.vertices,Int32[],Int32[],triangles,msh.colors)
end

function convert_and_duplicate(::Type{UnityMesh},msh::PyramidMesh,pattern::Vector{Int})
    id = msh.id
    verts = msh.vertices
    lines = Int32[]
    points = Int32[]
    inds = msh.pyramids
    clrs = msh.colors
    @assert mod(length(inds),4) == 0

    #new mesh
    #triangles = Vector{Int32}()
    vertices = Vector{Vector3}()
    colors = Vector{ColorTypes.RGBA{Float32}}()

    for i = 1:4:length(inds)

        tmpinds = inds[i:i+3]+1
        tmpverts = verts[tmpinds]
        tmpclrs = clrs[tmpinds]

        append!(vertices, tmpverts[pattern+1])
        append!(colors, tmpclrs[pattern+1])

    end

    triangles = Int32[i-1 for i = 1:length(vertices)]

    return UnityMesh(id,vertices,points,lines,triangles,colors)

end

function Base.write(tcpstream::TCPSocket, um::PyramidMesh)
    jum = JSON.json(convert(UnityMesh,um))
    return write(tcpstream, jum)
end

import Combinatorics
function boundary(upm::PyramidMesh)
    verts = Vector{GeometryTypes.Point3f0}()
    pyramids = Vector{Int32}()
    clrs = Vector{ColorTypes.RGBA{Float32}}()
    for i = 1:4:length(upm.pyramids)
        # todo
    end
end
end #module Unity
