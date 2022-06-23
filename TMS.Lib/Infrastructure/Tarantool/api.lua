local cartridge = require('cartridge')
local crud = require('crud')

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
    crud.insert_object('records', {id = record[1][1], parent_id = record[1][2], header_id = record[1][3], data = record[2] })
end


function get_record_by_id(id)
    crud.select('records', {{'=', 'id', id}})
end

function test(smth)
    return smth
end

return {
    test = test,
    add_records = add_records,
    get_record_by_id = get_record_by_id,
}