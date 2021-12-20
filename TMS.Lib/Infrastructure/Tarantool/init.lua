box.cfg();
s = box.schema.create_space('test_db');
s:create_index('primary', {
                type = 'hash',
                parts = {1, 'NUM'}
               });