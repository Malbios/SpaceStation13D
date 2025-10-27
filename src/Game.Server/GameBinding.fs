namespace Game.Server

open Ecs
open Game.Server.Game

module GameBinding =
    // Expose C#-friendly wrappers
    type World = Game.World

    let CreateWorld () = World.create ()
    let TickMovement (w:World, dt:single) = Systems.tickMovement dt w
    let SetDoorOpen (w:World, entityId:int, ``open``:bool) = Systems.setDoorOpen (EntityId entityId) ``open`` w

    let GetDoorOpen (w:World, entityId:int) =
        if has w.doors (EntityId entityId) then
            let i = w.doors.sparse[entityId]
            w.doors.denseData[i].IsOpen
        else false

    let SpawnTestEntities (w:World) =
        // Spawn one door and one mover
        let door = newStore () |> ignore
        let dId = newEntity w.entities
        add w.doors (EntityId dId) { Door.IsOpen = false; Access = 0 }
        add w.transforms (EntityId dId) { Transform.X = 0f; Y = 0f; Z = 2f }

        let pId = newEntity w.entities
        add w.transforms (EntityId dId) { Transform.X = 0f; Y = 0f; Z = 0f }
        add w.velocities (EntityId dId) { Velocity.VX = 0f; VY = 0f; VZ = 1f }
        ()
