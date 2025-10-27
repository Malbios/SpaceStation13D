namespace Game.Server

open Ecs

module Game =
    
    [<Struct>]
    type Transform = { mutable X: single; mutable Y: single; mutable Z: single }
    [<Struct>]
    type Velocity = { mutable VX: single; mutable VY: single; mutable VZ: single }
    [<Struct>]
    type Door = { mutable IsOpen: bool; Access:int }

    type World =
      { entities : EntityStore
        transforms : SparseSet<Transform>
        velocities : SparseSet<Velocity>
        doors : SparseSet<Door> }

    module World =
        let create () =
          { entities = newStore ()
            transforms = createSparseSet()
            velocities = createSparseSet()
            doors = createSparseSet() }

    module Systems =
        let tickMovement (dt:single) (w:World) =
            // iterate velocities dense array, update transforms if present
            let mutable i = 0
            while i < w.velocities.dense.Count do
                let e = w.velocities.dense[i]
                if has w.transforms e then
                    let idxT = w.transforms.sparse[EntityId.value e]
                    let v = w.velocities.denseData[i]
                    let t = w.transforms.denseData[idxT]
                    w.transforms.denseData[idxT] <-
                        { X = t.X + v.VX * dt
                          Y = t.Y + v.VY * dt
                          Z = t.Z + v.VZ * dt }
                i <- i + 1
            w

        let setDoorOpen (e:EntityId) (``open``:bool) (w:World) =
            
            let e = EntityId.value e
            
            if has w.doors (EntityId e) then
                let i = w.doors.sparse[e]
                let d = w.doors.denseData[i]
                w.doors.denseData[i] <- { d with IsOpen = ``open`` }
            w
