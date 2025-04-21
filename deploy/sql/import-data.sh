#aguardando 10 segundos para aguardar o provisionamento e start do banco
sleep 10s
#rodar o comando para criar o banco
/opt/mssql-tools18/bin/sqlcmd -S localhost,1433 -U SA -P "yourStrong(!)Password" -i criacao-banco-docker.sql -C