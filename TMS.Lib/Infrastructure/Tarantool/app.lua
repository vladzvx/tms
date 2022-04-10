local function stop()
	box.schema.space.create("mongos")
    box.space.mongos:create_index("mongo_id_part1",{type = 'TREE', parts = {{field = 1, type = 'integer'}}})
    box.space.mongos:create_index("mongo_id_part2",{type = 'TREE', parts = {{field = 2, type = 'integer'}}})
    box.space.mongos:create_index("entity",{type = 'TREE', parts = {{field = 3, type = 'unsigned'}}})
    box.space.mongos:create_index("type",{type = 'TREE', parts = {{field = 4, type = 'unsigned'}}})
    box.space.mongos:create_index("time",{type = 'TREE', parts = {{field = 5, type = 'integer'}}})

    box.schema.space.create("tmp")
    box.space.tmp:create_index("time",{type = 'TREE', parts = {{field = 1, type = 'integer'}}})

    box.schema.space.create("header")
    box.space.header:create_index("id",{type = 'TREE', parts = {{field = 1, type = 'integer'}}})
end

function test(header, protos)

    local protocols_count = table.maxn(protos)
    box.space.header:insert(header);
    if protocols_count>0 then
        for i=1,protocols_count,1 do 
            local prot = protos[i]
            box.space.tmp:insert(prot);
        end
    end
    return protocols_count;
end

box.cfg{} 
box.once('init', stop)