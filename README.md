# rinha-backend-2-quack

Repositório do meu projeto para a rinha de backend 2

Utilizei nginx, c# e mongodb

escolhi o c# pq precisava estudar pra usar no trabalho
e o mongodb foi pra poder armazenar as ultimas 10 transacoes num nested array e economizar nas consultas de extrato

Repo Rinha 2: https://github.com/zanfranceschi/rinha-de-backend-2024-q1

Run:
> git clone https://github.com/jorgehrique/rinha-backend-2-quack .
> docker build -t jorgehrique/rinha-backend-app-24-q1:1.0.0 rinha-app
> docker-compose up -d
base url: localhost:9999

resultado que tive (na minha máquina funciona)

---- Global Information --------------------------------------------------------
> request count                                      61503 (OK=61503  KO=0     )
> min response time                                      1 (OK=1      KO=-     )
> max response time                                    751 (OK=751    KO=-     )
> mean response time                                     5 (OK=5      KO=-     )
> std deviation                                         22 (OK=22     KO=-     )
> response time 50th percentile                          3 (OK=3      KO=-     )
> response time 75th percentile                          3 (OK=3      KO=-     )
> response time 95th percentile                          4 (OK=4      KO=-     )
> response time 99th percentile                         74 (OK=74     KO=-     )
> mean requests/sec                               251.033 (OK=251.033 KO=-     )
---- Response Time Distribution ------------------------------------------------
> t < 800 ms                                         61503 (100%)
> 800 ms <= t < 1200 ms                                  0 (  0%)
> t >= 1200 ms                                           0 (  0%)
> failed                                                 0 (  0%)
