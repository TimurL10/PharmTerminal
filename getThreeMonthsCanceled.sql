
declare @aptguid uniqueidentifier, @oneStartdate date, @oneEnddate date, @twoStartdate date, @twoEnddate date,
@threeStartdate date, @threeEnddate date, @fourStartdate date, @fourEnddate date,
@fiveStartdate date, @fiveEnddate date, @sixStartdate date, @sixEnddate date,
@sevenStartdate date, @sevenEnddate date, @eightStartdate date, @eightEnddate date,
@nineStartdate date, @nineEnddate date, @tenStartdate date, @tenEnddate date,
@elevenStartdate date, @elevenEnddate date, @twelfthStartdate date, @twelfthEnddate date

set @oneStartdate = '2020-11-01'
set @oneEnddate = '2020-11-07'
set @twoStartdate = '2020-11-08'
set @twoEnddate = '2020-11-15'
set @threeStartdate = '2020-11-16'
set @threeEnddate = '2020-11-23'
set @fourStartdate = '2020-11-24'
set @fourEnddate = '2020-12-01'
set @fiveStartdate = '2020-12-02'
set @fiveEnddate = '2020-12-09'
set @sixStartdate = '2020-12-16'
set @sixEnddate = '2020-12-17'
set @sevenStartdate = '2020-12-24'
set @sevenEnddate = '2020-12-25'
set @eightStartdate = '2021-01-01'
set @eightEnddate = '2021-01-07'
set @nineStartdate = '2021-01-08'
set @nineEnddate = '2021-01-15'
set @tenStartdate = '2021-01-16'
set @tenEnddate = '2021-01-23'
set @elevenStartdate = '2021-01-24'
set @elevenEnddate = '2021-01-31'
set @twelfthStartdate = '2021-01-10'
set @twelfthEnddate = '2021-01-20'


SELECT one.num, two.num, three.num, four.num, five.num, six.num, seven.num, eight.num, nine.num, ten.num, eleven.num, twelfth.num
FROM   
(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @oneStartdate  and @oneEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) one,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @twoStartdate  and @twoEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) two,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @threeStartdate  and @threeEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) three,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @fourStartdate  and @fourEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) four,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @fiveStartdate  and @fiveEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) five,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @sixStartdate  and @sixEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) six,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @sevenStartdate  and @sevenEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) seven,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @eightStartdate  and @eightEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) eight,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @nineStartdate  and @nineEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) nine,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @tenStartdate  and @tenEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) ten,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @elevenStartdate  and @elevenEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) eleven,

(select COUNT(h.number) as num
from[Documents].[OrderHeaders] h
inner join[Documents].[OrderStatuses] s on h.orderid = s.orderid
where h.storeid in
(SELECT[TableRowGUID]
FROM[References].PN_PharmacySync p
JOIN[References].[UnionNetSync] n ON n.id = p.id_pn_unionnet
WHERE n.Real_Net_Guid = 'efb05410-ba92-4a73-a37f-f05f9a499ded'
AND p.Actual = 1)
and h.date between @twelfthStartdate  and @twelfthEnddate and s.status = 100
and h.orderid in (select orderid from[Documents].[OrderStatuses] where[status] in (202))
HAVING COUNT(h.number) > 0) twelfth


