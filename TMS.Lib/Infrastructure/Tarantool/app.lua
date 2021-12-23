local function stop()
	box.schema.space.create("mongos")
    box.space.mongos:create_index("mongo_id_part1",{type = 'TREE', parts = {{field = 1, type = 'integer'}}})
    box.space.mongos:create_index("mongo_id_part2",{type = 'TREE', parts = {{field = 2, type = 'integer'}}})
    box.space.mongos:create_index("entity",{type = 'TREE', parts = {{field = 3, type = 'unsigned'}}})
    box.space.mongos:create_index("type",{type = 'TREE', parts = {{field = 4, type = 'unsigned'}}})
    box.space.mongos:create_index("time",{type = 'TREE', parts = {{field = 5, type = 'integer'}}})
end

box.cfg{} 
box.once('init', stop)