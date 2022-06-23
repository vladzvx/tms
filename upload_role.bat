pscp.exe -pw сpQ7C5Vr0j "C:\work\tms\TMS.Lib\Infrastructure\Tarantool\test.lua" "root@195.133.196.149:/test_app1/app/roles/test.lua"

Plink root@195.133.196.149 -no-antispoof -pw сpQ7C5Vr0j "cd /test_app1"; "cartridge stop"; "cartridge build"; "cartridge start -d"; 