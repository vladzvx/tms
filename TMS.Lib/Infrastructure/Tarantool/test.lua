local cartridge = require('cartridge')
local crud = require('crud')
local role_name = 'test'

local function init(opts)
    if opts.is_master then
        local space = box.schema.space.create('records', {
                format = {
                    {name = 'id', type = 'unsigned'},
                    {name = 'bucket_id', type = 'unsigned'},
                    {name = 'parent_id', type = 'unsigned'},
                    {name = 'header_id', type = 'unsigned'},
                    {name = 'data', type = 'double'},
                },
                if_not_exists = true,
            });
            space:create_index('id', {
                parts = { {field ='id', is_nullable = false} },
                if_not_exists = true,
            });
            space:create_index('bucket_id', {
                parts = { {field ='bucket_id', is_nullable = false} },
                if_not_exists = true,
            })
    end
end

local function stop()

end

function add_records(records)
    add_records_core(records)
end

function add_records_core(records)
    local records_count = table.maxn(records)
    if records_count>0 then
        for i=1,records_count,1 do 
            parse_and_insert(records[i]);
        end
    end
end

function parse_and_insert(record)
    crud.insert_object('records', {id = record[1][1], parent_id = record[1][2], header_id = record[1][3], data =record[2]  })
   --box.space.records:insert{record[1][1],record[1][1],record[1][2],record[1][3],record[2] }
    -- crud.insert('records', {record[1][1],record[1][2],record[1][3],record[2] })
end


function get_record_by_id(id)
    return crud.select('records', {{'=', 'id', id}})
end



return {
    init = init,
    stop = stop,
    role_name = role_name,
    add_records = add_records,
    get_record_by_id = get_record_by_id,
}