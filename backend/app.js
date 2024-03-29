const express = require("express");
const bodyParser = require("body-parser");
const path = require('path');
const request = require('request');
const multer = require("multer");
const storage = multer.diskStorage({
    destination: (req, file, callback) => {
        callback(null, 'images');
    },
    filename: (req, file, callback) => {
        console.log(file);
        callback(null, Date.now() + path.extname(file.originalname));
    }

})
const upload = multer({ storage: storage });

let mysql = require("mysql");
const { json } = require("body-parser");
const { error } = require("console");

let connection = mysql.createConnection({
    host: 'vrestate.clbfq7mnbhip.eu-west-3.rds.amazonaws.com',
    port: '3306',
    user: 'admin',
    password: 'VRestate123**',
    database: 'vrestate'
});
const app = express();
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.use(function (req, res, next) {
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type');
    res.setHeader('Access-Control-Allow-Credentials', true);
    next();
});

app.get('/testapi', (req, res) => {
    let query = 'SELECT name FROM vrestate.test WHERE id = ' + req.query.id;

    connection.query(query, (error, results, fields) => {
        if (error) {
            return console.error(error);
        }
        if (results.length == 0) {
            res.send('Kullanici Bulunamadi!');
        }
        else {
            res.send((req.query.id) + ' nolu id ye sahip kullanici ' + results[0].name);
            console.log(results[0].name)
        }

    });


});

app.get('/loaderio-0ea84b7e34801498d9390d93deb66294', (req, res) => {
    res.send('loaderio-0ea84b7e34801498d9390d93deb66294');
})

app.get('/apitest', (req, res) => {
    res.json({
        "keys": ["id"],
        "values": [
            ["1"],
            ["2"],
            ["3"],
            ["4"],
            ["5"],
            ["6"],
            ["7"],
            ["8"],
            ["9"],
            ["41"]
        ]
    });
});


//test için html donen endpoint. frontendi tamamlandığında silinecek
/*
app.get('/imageupload', function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.write('<form action="/upload" enctype="multipart/form-data" method="post">');
    res.write(' <input type="file" name="image" accept="image/*">');
    res.write('<input type="submit" value="Upload">');
    res.write('</form>');
    return res.end();
})
*/

app.post('/upload', upload.single("file"), (req, res) => {
    if (!req.file) {
        console.log("No file upload");
    } else {
        console.log(req.file.filename)
        var imgsrc = req.file.filename
        var insertData = "INSERT INTO photos(file_src) VALUES (?)"
        connection.query(insertData, [imgsrc], (err, result) => {
            if (err) throw err
            console.log("file uploaded")
            connection.query('select file_id from photos order by 1 desc', (error, results, fields) => {
                if (err) throw err
                res.send({
                    message: "File Uploaded",
                    id: results[0].file_id
                });
            })
        })
    }
});

app.get('/getImage', (req, res) => {
    if (req.query.id == null || req.query.id == '') {
        res.status(400).send({ message: 'FileID can not be null' })
        return;
    }
    let query = 'select * from photos where file_id =' + req.query.id;
    connection.query(query, (error, results) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        if (results.length == 0) {
            res.send('Image Not Found');
            return;
        }
        res.sendFile('images/' + results[0].file_src, { root: '.' });
    })
})


app.get('/login/check', (req, res) => {
    let query = 'SELECT * FROM vrestate.user WHERE mail = \'' + req.query.mail + '\'';

    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error: ' + error;
            res.status(500).end();
            return console.error(error);
        }
        if (results.length == 0) {
            res.statusMessage = 'User Not Found';
            res.status(400).end();
        }
        else {
            res.send(results[0]);
        }

    });


});

app.get('/login/getById', (req, res) => {
    let query = 'SELECT * FROM user inner join profile on user.id = profile.id  WHERE user.id = ' + req.query.id;

    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error: ' + error;
            res.status(500).end();
            return console.error(error);
        }
        if (results.length == 0) {
            res.statusMessage = 'User Not Found';
            res.status(400).end();
        }
        else {
            res.send(results[0]);
        }

    });


});

app.get('/profile/getProfile', (req, res) => {
    if (req.query.id == null || req.query.id == undefined) {
        res.status(400).send({ message: "Params cannot be null" });
        return;
    }
    let query = 'select * from user inner join profile on user.id = profile.id where user.id = ' + req.query.id;
    connection.query(query, (error, results) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        res.status(200).send(results);
    })
})

app.post('/profile/update', (req, res) => {
    if (req.query.id == null || req.query.id == undefined) {
        res.status(400).send({ message: "Params cannot be null" });
        return;
    }
    let values = []
    values[values.length] = req.body.TC_no;
    values[values.length] = req.body.photo_id;
    values[values.length] = req.body.paymentInfo;
    values[values.length] = req.body.profileType;
    values[values.length] = req.body.phone;
    let query = 'update profile set TC_no = ?, photo_id = ?, paymentInfo = ?, profileType = ?, phone = ? where id = ' + req.query.id;
    connection.query(query, values, (error, results) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        res.status(200).send({ message: 'User Profile Successfully Updated.' });
    })

})

app.get('/profile/updateLocation', (req, res) => {
    if (req.query.id == null || req.query.id == undefined) {
        res.status(400).send({ message: "Params cannot be null" });
        return;
    }
    let currentLocation;
    let ip = req.socket.remoteAddress.substring(req.socket.remoteAddress.indexOf(':', 2) + 1);
    //let ip = "176.89.195.54";
    request('http://ip-api.com/json/' + ip, (error, response, body) => {
        if (error) {
            currentLocation = null;
        }
        console.log((JSON.parse(body)))
        console.log((JSON.parse(body)).regionName)
        currentLocation = ((JSON.parse(body)).regionName);
        let query = 'update profile set currentLocation = \'' + currentLocation + '\' where id = ' + req.query.id;
        connection.query(query, (error, results) => {
            if (error) {
                res.statusMessage = 'Database Query Error';
                res.status(500).send({ message: error });
                return;
            }
            res.status(200).send({ message: 'User Location Successfully Updated.' });
        })
    });
})

app.post('/profile/updateName', (req, res) => {
    if (req.query.id == null || req.query.id == undefined || req.body.name == null || req.body.name == undefined || req.body.surname == null || req.body.surname == undefined) {
        res.status(400).send({ message: "Params cannot be null" });
        return;
    }
    let query = 'update user set name = ' + req.body.name + ', surname = ' + req.body.surname + ' where id = ' + req.query.id;
    connection.query(query, (error, results) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        res.status(200).send({ message: 'User Name/Surname Successfully Updated.' });
    })
})

app.post('/login', (req, res) => {
    let query = 'SELECT * FROM vrestate.user WHERE mail = \'' + req.body.mail + '\'';

    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error: ' + error;
            res.status(500).end();
            return console.error(error);
        }
        if (results.length == 0) {
            let query2 = 'INSERT INTO user (id,name,surname,mail) values (\'' + req.body.id + '\',\'' + req.body.name + '\',\'' + req.body.surname + '\',\'' + req.body.mail + '\')';
            connection.query(query2, (error, results, fields) => {
                if (error) {
                    res.statusMessage = 'Database Query Error: ' + error;
                    res.status(500).end();
                    return console.error(error);
                }
                else {
                    let currentLocation;
                    let ip = req.socket.remoteAddress.substring(req.socket.remoteAddress.indexOf(':', 2) + 1);
                    //let ip = "176.89.195.54";
                    request('http://ip-api.com/json/' + ip, (error, response, body) => {
                        if (error) {
                            currentLocation = null;
                        }
                        console.log((JSON.parse(body)).regionName)
                        currentLocation = ((JSON.parse(body)).regionName);
                        let query3 = 'INSERT INTO profile (id, createDate, currentLocation, profileType) values (\'' + req.body.id + '\',\'' + new Date().toISOString() + '\',\'' + currentLocation + '\',\'' + 'Standard' + '\')';
                        connection.query(query3, (error, results, fields) => {
                            if (error) {
                                res.statusMessage = 'Database Query Error: ' + error;
                                res.status(500).end();
                                return console.error(error);
                            }
                            res.status(200);
                            res.send('User Successfully Created!')
                        })
                    })

                }

            });
        }
        else {
            res.send(results[0]);
        }

    });


});

//girilmeyen değerleri apiye null gönderelim.
app.post('/estate/create', (req, res) => {
    let query = "INSERT INTO estate (title, head_photo_id, estate_type, category, price, create_date, last_update, location_ilce, location_il, coordX, coordY, room_type, m2, vr_id, owner_id) VALUES (?)"
    let values = [];
    values[values.length] = req.body.title;
    values[values.length] = req.body.head_photo_id;
    values[values.length] = req.body.estate_type;
    values[values.length] = req.body.category;
    values[values.length] = req.body.price;
    let date = new Date().toISOString()
    values[values.length] = date;
    values[values.length] = date;
    values[values.length] = req.body.ilce;
    values[values.length] = req.body.il;
    values[values.length] = req.body.coordX;
    values[values.length] = req.body.coordY;
    values[values.length] = req.body.room_type;
    values[values.length] = req.body.m2;
    values[values.length] = req.body.vr_id;
    values[values.length] = req.body.owner_id;
    let createdId;
    connection.query(query, [values], (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        query = 'select * from estate order by 1 desc'
        connection.query(query, (error, results, fields) => {
            if (error) {
                res.statusMessage = 'Database Query Error';
                res.status(500).send({ message: error });
                return;
            }
            createdId = results[0].id;
            query = "INSERT INTO estate_detail (id, photo_ids, m2_brut, buildingAge, floors, buildingFloors, heatingSystem, balconyCount, bathroomCount, isFurnished, isBuildingComplex, buildingFees, complexName, isTradeable) VALUES (?)"
            values = [];
            values[values.length] = createdId;
            values[values.length] = req.body.photo_ids;
            values[values.length] = req.body.m2_brut;
            values[values.length] = req.body.buildingAge;
            values[values.length] = req.body.floors;
            values[values.length] = req.body.buildingFloors;
            values[values.length] = req.body.heatingSystem;
            values[values.length] = req.body.balconyCount;
            values[values.length] = req.body.bathroomCount;
            values[values.length] = req.body.isFurnished;
            values[values.length] = req.body.isBuildingComplex;
            values[values.length] = req.body.buildingFees;
            values[values.length] = req.body.complexName;
            values[values.length] = req.body.isTradeable;

            connection.query(query, [values], (error, results, fields) => {
                if (error) {
                    res.statusMessage = 'Database Query Error';
                    res.status(500).send({ message: error });
                    return;
                }
                res.status(200).send({ id: createdId, message: 'Estate Successfully Created' });
                return;
            })
        })

    })
});

app.post('/estate/update', (req, res) => {
    if (req.query.id == null || req.query.ownerId == null) {
        res.status(400).send({ message: "Params cannot be null" });
        return;
    }
    let values = [];
    values[values.length] = req.query.id
    values[values.length] = req.query.ownerId
    let query = 'select id from estate where id = ? and owner_id = ?';
    connection.query(query, values, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        if (results.length == 0) {
            res.status(400).send({ message: "No estate found to update" });
            return;
        }
        values = [];
        values[values.length] = req.body.title;
        values[values.length] = req.body.head_photo_id;
        values[values.length] = req.body.estate_type;
        values[values.length] = req.body.category;
        values[values.length] = req.body.price;
        let date = new Date().toISOString().substring(0, 10);
        values[values.length] = date;
        values[values.length] = req.body.ilce;
        values[values.length] = req.body.il;
        values[values.length] = req.body.coordX;
        values[values.length] = req.body.coordY;
        values[values.length] = req.body.room_type;
        values[values.length] = req.body.m2;
        values[values.length] = req.body.vr_id;
        values[values.length] = req.query.id;
        values[values.length] = req.query.ownerId;
        query = 'update estate set title = ?, head_photo_id = ?, estate_type = ?, category = ?, price = ?, last_update = ?, location_ilce = ?, location_il = ?, coordX = ?, coordY = ?, room_type = ?, m2 = ?, vr_id = ? where id = ? and owner_id = ?'
        connection.query(query, values, (error, results, fields) => {
            if (error) {
                res.statusMessage = 'Database Query Error';
                res.status(500).send({ message: error });
                return;
            }
            values = [];
            values[values.length] = req.body.photo_ids;
            values[values.length] = req.body.m2_brut;
            values[values.length] = req.body.buildingAge;
            values[values.length] = req.body.floors;
            values[values.length] = req.body.buildingFloors;
            values[values.length] = req.body.heatingSystem;
            values[values.length] = req.body.balconyCount;
            values[values.length] = req.body.bathroomCount;
            values[values.length] = req.body.isFurnished;
            values[values.length] = req.body.isBuildingComplex;
            values[values.length] = req.body.buildingFees;
            values[values.length] = req.body.complexName;
            values[values.length] = req.body.isTradeable;
            values[values.length] = req.query.id;
            query = 'update estate_detail set photo_ids = ?, m2_brut = ?, buildingAge = ?, floors = ?, buildingFloors = ?, heatingSystem = ?, balconyCount = ?, bathroomCount = ?, isFurnished = ?, isBuildingComplex = ?, buildingFees = ?, complexName = ?, isTradeable = ? where id = ?'
            connection.query(query, values, (error, results, fields) => {
                console.log(query)
                if (error) {
                    res.statusMessage = 'Database Query Error';
                    res.status(500).send({ message: error });
                    return;
                }
                res.status(200).send({ message: "Estate Successfully Updated" });
            })
        })
    })

});

app.post('/estate/delete', (req, res) => {
    if (req.query.id == null || req.query.ownerId == null) {
        res.status(400).send({ message: "Params cannot be null" });
        return;
    }
    let values = [];
    values[values.length] = req.query.id
    values[values.length] = req.query.ownerId
    let query = 'select id from estate where id = ? and owner_id = ?';
    connection.query(query, values, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        if (results.length == 0) {
            res.status(400).send({ message: "No estate found to delete" });
            return;
        }
        query = 'DELETE FROM estate_detail WHERE id = ' + req.query.id;
        connection.query(query, (error, results, fields) => {
            if (error) {
                res.statusMessage = 'Database Query Error';
                res.status(500).send({ message: error });
                return;
            }
            query = 'DELETE FROM estate WHERE id = ' + req.query.id;
            connection.query(query, (error, results, fields) => {
                if (error) {
                    res.statusMessage = 'Database Query Error';
                    res.status(500).send({ message: error });
                    return;
                }
                res.status(200).send({ message: "Estate Successfully Deleted" });
            })
        })
    })
});

app.get('/estate/getEstates', (req, res) => {
    let query = 'Select * from estate'
    if (req.query.detail == 'true') {
        if (req.query.user == 'true') {
            query = 'select estate.*, estate_detail.*, user.name, user.surname, user.mail from estate inner join estate_detail on estate.id = estate_detail.id inner join user on user.id = estate.owner_id'
            if (req.query.userDetail == 'true') {
                query = 'select estate.*, estate_detail.*, user.name, user.surname, user.phone, user.mail, profile.TC_no, profile.photo_id, profile.createDate, profile.currentLocation, profile.paymentInfo, profile.profileType, profile.phone from estate inner join estate_detail on estate.id = estate_detail.id inner join user on user.id = estate.owner_id '
                query += ' inner join profile on profile.id = estate.owner_id'
            }
        }
        else {
            query += ' inner join estate_detail on estate.id = estate_detail.id'
        }
    }

    if (req.query.searchFilter == 'true') {
        query = query + ' where'
        if (req.query.id != null || req.query.id != undefined) {
            query += ' estate.id = ' + req.query.id + ' and';
        }

        if (req.query.title != null || req.query.title != undefined) {
            query += ' title like \'%' + req.query.title + '%\' and';
        }

        if (req.query.estate_type != null || req.query.estate_type != undefined) {
            query += ' estate_type = ' + req.query.estate_type + ' and';
        }

        if (req.query.category != null || req.query.category != undefined) {
            query += ' category = ' + req.query.category + ' and';
        }

        if (req.query.priceMin != null || req.query.priceMin != undefined) {
            query += ' price >= ' + req.query.priceMin + ' and';
        }

        if (req.query.priceMax != null || req.query.priceMax != undefined) {
            query += ' price <= ' + req.query.priceMax + ' and';
        }

        if (req.query.create_date != null || req.query.create_date != undefined) {//todo date min max
            //query+= ' id = '+req.query.id+' and';
        }

        if (req.query.location_il != null || req.query.location_il != undefined) {
            query += ' location_il = ' + req.query.location_il + ' and';
        }

        if (req.query.location_ilce != null || req.query.location_ilce != undefined) {
            query += ' location_ilce = ' + req.query.location_ilce + ' and';
        }

        if (req.query.room_type != null || req.query.room_type != undefined) {
            query += ' room_type = ' + req.query.room_type + ' and';
        }

        if (req.query.price != null || req.query.price != undefined) {//todo minmax
            //query+= ' id = '+req.query.id+' and';
        }
    }
    if (query.search('and') > 0) {
        query = query.substring(0, query.length - 3);
    }


    console.log(query);
    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        res.status(200).send({
            results
        })
    })
});

app.get('/estate/getEstatesH', (req, res) => {
    let query = 'Select * from hepsi_estate'
    if (req.query.searchFilter == 'true') {
        query = query + ' where'
        if (req.query.id != null || req.query.id != undefined) {
            query += ' estate.id = ' + req.query.id + ' and';
        }

        if (req.query.title != null || req.query.title != undefined) {
            query += ' title like \'%' + req.query.title + '%\' and';
        }

        if (req.query.url != null || req.query.url != undefined) {
            query += ' url like \'%' + req.query.url + '%\' and';
        }

        if (req.query.estate_type != null || req.query.estate_type != undefined) {
            query += ' estate_type = ' + req.query.estate_type + ' and';
        }

        if (req.query.category != null || req.query.category != undefined) {
            query += ' category = ' + req.query.category + ' and';
        }

        if (req.query.priceMin != null || req.query.priceMin != undefined) {
            query += ' price >= ' + req.query.priceMin + ' and';
        }

        if (req.query.priceMax != null || req.query.priceMax != undefined) {
            query += ' price <= ' + req.query.priceMax + ' and';
        }

        if (req.query.create_date != null || req.query.create_date != undefined) {//todo date min max
            //query+= ' id = '+req.query.id+' and';
        }

        if (req.query.location_il != null || req.query.location_il != undefined) {
            query += ' location_il = ' + req.query.location_il + ' and';
        }

        if (req.query.location_ilce != null || req.query.location_ilce != undefined) {
            query += ' location_ilce = ' + req.query.location_ilce + ' and';
        }

        if (req.query.room_type != null || req.query.room_type != undefined) {
            query += ' room_type = ' + req.query.room_type + ' and';
        }

        if (req.query.price != null || req.query.price != undefined) {//todo minmax
            //query+= ' id = '+req.query.id+' and';
        }
    }
    if (query.search('and') > 0) {
        query = query.substring(0, query.length - 3);
    }


    console.log(query);
    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        res.status(200).send({
            results
        })
    })
});

//json mu gidiyor control.
app.get('/checkLocation', (req, res) => {
    let ip = req.socket.remoteAddress.substring(req.socket.remoteAddress.indexOf(':', 2) + 1);
    request('http://ip-api.com/json/' + ip, (error, response, body) => {
        if (error) {
            res.status(400).send({ message: error });
            return;
        }
        let json = JSON.parse(body);
        console.log(json);
        res.send(body)
        //res.send({country:body.country,region:body.regionName});
    })

})

app.post('/unity/assignId', (req, res) => {
    let id = Math.floor(100000 + Math.random() * 900000);
    let query = 'select id from vr_model'
    connection.query(query, (error, results, fields) => {
        console.log(1)
        console.log(results)
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        let flag = true;
        while (flag) {
            console.log('while')
            for (let i = 0; i < results.length; i++) {
                console.log(i + ' i')
                console.log(results[i])
                if (id == results[i].id) {
                    console.log('aynı id denk geldi tekrar.')
                    flag = true;
                    id = Math.floor(100000 + Math.random() * 900000);
                    break;
                }
                if (i == results.length - 1) {
                    flag = false;
                }
            }
        }
        query = 'insert into vr_model(id, model) values(' + id + ',null)'
        connection.query(query, (error, results, fields) => {
            if (error) {
                res.statusMessage = 'Database Query Error';
                res.status(500).send({ message: error });
                return;
            }
            res.status(200).send({ message: 'VR ID successfully created.', id: id })
        })
    })



});

app.post('/unity/save', (req, res) => {
    if (req.query.id == null) {
        res.status(400).send({ message: 'ID cannot be null!' });
        return;
    }
    let query = 'select id from vr_model where id = ' + req.query.id;
    console.log(req.body)
    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        if (results.length == 0) {
            res.status(400).send({ message: 'VR model does not exist' });
            return;
        }
        query = 'update vr_model set model = ? where id = ' + req.query.id;
        let values = JSON.stringify(req.body);
        connection.query(query, values, (error, results, fields) => {
            if (error) {
                res.statusMessage = 'Database Query Error';
                res.status(500).send({ message: error });
                return;
            }
            res.status(200).send({ message: 'Model successfully saved.' })
        })
    })
})

app.get('/unity/load', (req, res) => {
    if (req.query.id == null) {
        res.status(400).send({ message: 'ID cannot be null!' });
        return;
    }
    let query = 'select * from vr_model where id = ' + req.query.id;
    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        if (results.length == 0) {
            res.status(400).send({ message: 'VR model does not exist' });
            return;
        }
        let jsonReply = JSON.parse(results[0].model);
        res.status(200).json(jsonReply)
    })

})

app.get('/unity/userDetails', (req,res) => {
    if (req.query.ownerId == null) {
        res.status(400).send({ message: 'Owner ID cannot be null!' });
        return;
    }
    let query = 'select * from user inner join profile on user.id = profile.id where user.id = '+req.query.ownerId;
    connection.query(query, (error,results) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        res.status(200).send(results[0]);
    })
})

app.get('/unity/estateDetails', (req, res) => {
    if (req.query.modelId == null) {
        res.status(400).send({ message: 'Model ID cannot be null!' });
        return;
    }
    let query = 'Select * from estate inner join estate_detail on estate.id = estate_detail.id where estate.vr_id = ' + req.query.modelId + '';
    connection.query(query, (error, results, fields) => {
        if (error) {
            res.statusMessage = 'Database Query Error';
            res.status(500).send({ message: error });
            return;
        }
        if (results.length == 0) {
            query = 'Select * from hepsi_estate where vr_id = ' + req.query.modelId + '';
            connection.query(query, (error, results) => {
                if (error) {
                    res.statusMessage = 'Database Query Error';
                    res.status(500).send({ message: error });
                    return;
                }
                if (results.length == 0) {
                    res.status(400).send({ message: 'Estate does not exist' });
                    return;
                }
                res.status(200).send(results[0]);
                return;
            })
            return;
        }
        res.status(200).send(results[0]);
        return;
    })

})

app.listen(5002, () => {
    connection.connect(function (err) {
        if (err) {
            return console.error('error: ' + err.message);
        }

        console.log('Connected to the MySQL server.');
    });
    console.log('server started on port 5002');
});