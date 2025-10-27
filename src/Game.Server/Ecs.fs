namespace Game.Server

module Ecs =
    
    [<Struct>]
    type EntityId = EntityId of int with
        member x.Value = let (EntityId v) = x in v
        
    [<RequireQualifiedAccess>]
    module EntityId =
        let value (EntityId v) = v

    type EntityStore = {
        mutable nextId:int; free: System.Collections.Generic.Stack<int>
    }
    
    let newStore () = {
        nextId = 1; free = System.Collections.Generic.Stack()
    }
    
    let newEntity (es:EntityStore) =
        if es.free.Count > 0 then es.free.Pop() else
        let id = es.nextId
        es.nextId <- id + 1
        id

    type SparseSet<'T> = {
        mutable dense : System.Collections.Generic.List<EntityId>
        mutable denseData : System.Collections.Generic.List<'T>
        mutable sparse : System.Collections.Generic.List<int>
    } // -1 = none

    let private resizeTo (lst:System.Collections.Generic.List<int>) (n:int) =
        
        let mutable i = lst.Count
        
        while i <= n do
            lst.Add(-1)
            i <- i + 1

    let createSparseSet<'T> () = {
        dense = System.Collections.Generic.List<EntityId>()
        denseData = System.Collections.Generic.List<'T>()
        sparse = System.Collections.Generic.List<int>()
    }
        
    let has (ss: SparseSet<'T>) (e: EntityId) =
        
        let e = EntityId.value e
        
        if e < ss.sparse.Count then ss.sparse[e] >= 0 else false

    let add (ss:SparseSet<'T>) (e:EntityId) (c:'T) =
        
        let e = EntityId.value e
        
        if e >= ss.sparse.Count then resizeTo ss.sparse e
        
        if ss.sparse[e] >= 0 then
            ss.denseData[ss.sparse[e]] <- c
        else
            let i = ss.dense.Count
            ss.dense.Add(EntityId e)
            ss.denseData.Add(c)
            ss.sparse[e] <- i

    let remove (ss:SparseSet<'T>) (e:EntityId) =
        
        let e = EntityId.value e
        
        if e < ss.sparse.Count then
            let i = ss.sparse[e]
            if i >= 0 then
                let last = ss.dense.Count - 1
                let lastEnt = ss.dense[last]
                ss.dense[i] <- lastEnt
                ss.denseData[i] <- ss.denseData[last]
                ss.sparse[EntityId.value lastEnt] <- i
                ss.dense.RemoveAt(last)
                ss.denseData.RemoveAt(last)
                ss.sparse[e] <- -1
