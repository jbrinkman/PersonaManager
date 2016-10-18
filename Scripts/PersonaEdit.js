var PersonaManager = PersonaManager || {};

PersonaManager.personaViewModel = function (moduleId, resx) {
    var service = {
        path: "PersonaManager",
        framework: $.ServicesFramework(moduleId)
    }
    service.baseUrl = service.framework.getServiceRoot(service.path) + "Persona/";

    var id = ko.observable(-1);
    var name = ko.observable('');
    var description = ko.observable('');
    var assignedUser = ko.observable(-1);
    var userList = ko.observableArray([]);
    var isLoading = ko.observable(false);

    var init = function () {
        var qs = getQueryStrings();
        var id = qs["tid"];
        if (id) {
            getPersona(id);
        }
        getUserList();
    };

    var getQueryStrings = function () {
        var assoc = {};
        var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
        var queryString = location.search.substring(1);
        var keyValues = queryString.split('&');

        for (var i = 0; i < keyValues.length; i++) {
            var key = keyValues[i].split('=');
            if (key.length > 1) {
                assoc[decode(key[0])] = decode(key[1]);
            }
        }

        return addRewriteQueryString(assoc, decode);
    };

    var addRewriteQueryString = function (hash, decode) {
        var path = location.pathname;
        var queryString = path.substring(path.search('/ctl/') + 1);
        var keyValues = queryString.split('/');

        for (var i = 0; i < keyValues.length; i += 2) {
            hash[decode(keyValues[i])] = decode(keyValues[i + 1])
        }

        return hash;
    };

    var getPersona = function (id) {
        isLoading(true);

        var restUrl = service.baseUrl + id;
        var jqXHR = $.ajax({
            url: restUrl,
            beforeSend: service.framework.setModuleHeaders,
            dataType: "json"
        }).done(function (data) {
            if (data) {
                load(data);
            }
            else {
                clear();
            }
        }).always(function (data) {
            isLoading(false);
        });
    };

    var getUserList = function () {
        isLoading(true);

        // need to calculate a different Url for User service
        var restUrl = service.framework.getServiceRoot(service.path) + "User/";
        var jqXHR = $.ajax({
            url: restUrl,
            beforeSend: service.framework.setModuleHeaders,
            dataType: "json",
            async: false
        }).done(function (data) {
            if (data) {
                loadUsers(data);
            }
            else {
                clear();
            }
        }).always(function (data) {
            isLoading(false);
        });
    };

    var load = function (data) {
        id(data.id)
        name(data.name);
        assignedUser(data.assignedUser);
        description(data.description);
    };

    var save = function () {
        isLoading(true);
        var persona = {
            id: id(),
            name: name(),
            description: description(),
            assignedUser: assignedUser()
        };
        var ajaxMethod = "POST";
        var restUrl = service.baseUrl;

        if (persona.id > 0) {
            // ajaxMethod = "PATCH";
            restUrl += persona.id;
        }
        var jqXHR = $.ajax({
            method: ajaxMethod,
            url: restUrl,
            contentType: "application/json; charset=UTF-8",
            data: JSON.stringify(persona),
            beforeSend: service.framework.setModuleHeaders,
            dataType: "json"
        }).done(function (data) {
            console.log(data);
            dnnModal.closePopUp();
        }).always(function (data) {
            isLoading(false);
        });
    };

    var loadUsers = function (data) {
        userList.removeAll();
        var underlyingArray = userList();
        for (var i = 0; i < data.length; i++) {
            var result = data[i];
            var user = new PersonaManager.user(result.id, result.name);
            underlyingArray.push(user);
        }
        userList.valueHasMutated();
    };

    var clear = function () {
        id('');
        name('');
        description('');
        assignedUser(-1);
    };

    var cancel = function () {
        dnnModal.closePopUp(false);
    };

    return {
        id: id,
        name: name,
        description: description,
        assignedUser: assignedUser,
        userList: userList,
        cancel: cancel,
        load: load,
        save: save,
        init: init,
        isLoading: isLoading
    };
}

PersonaManager.user = function (id, name) {
    this.id = id;
    this.name = name;
}