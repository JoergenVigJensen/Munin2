muninApp.controller('MapFormCtrl', function ($scope, $timeout, $http) {

    //https://angular-ui.github.io/ui-select/

    var vm = this;

    vm.mapTypes = [];
    vm.journaler = [];
    vm.materialList = [];

    vm.model = {};

    vm.journal = {};
    vm.material = '';
    vm.mapType = '';

    $scope.selectedItem = {};
    $scope.pageLoaded = false;

    $scope.changeMapType = function() {
        vm.model.mapType = parseInt(vm.mapType);
    }

    $scope.changeMaterial = function() {
        vm.model.material = parseInt(vm.material);
    }

    $scope.initpage = function () {
        var id = angular.element('#ID').val();
        $http.get('/Maps/load/' + id).then(function (result) {
            console.log(result);
            vm.model = result.data.model;
            if (result.data.model.journal != null) {
                vm.journal.value = vm.model.journal.journalId;
                vm.journal.text = vm.model.journal.journalNb;
            }
            vm.mapTypes = result.data.mapTypes;
            vm.materialList = result.data.materialList;
            vm.journaler = result.data.journalList;
            vm.mapType = vm.model.mapType.toString();
            vm.material = vm.model.material.toString();

            $scope.pageLoaded = true;
        },
        function (result) {
            vm.message = "Der opstod en fejl i forbindelse med at vise siden.";
            console.log(result);
        });
    }

    $scope.save = function () {
        if (vm.journal != null) {
            vm.model.journal = {
                journalId: vm.journal.value,
                journalNb: vm.journal.text
            };
        }
        if (vm.mapType != null) {
            vm.model.mapType = vm.mapType.value;
        }
        if (vm.material != null) {
            vm.model.material = vm.material.value;
        }

        $http.post('/Maps/save/', vm.model).then(function (result) {
            if (result.data.success === true)
                window.location.href = "/Sequences/index";
            vm.message = result.data.message;
        }, function (result) {
            console.log(result);
            vm.message = 'Der opstod en fejl.';
        });
    }

    $scope.getNewIndex = function () {
        $http.post('/Maps/getSesquenceNb/', {  }).then(function (result) {
            if (result.data.success === true) {
                vm.model.sequenceNb = result.data.output;
            }
            vm.message = result.data.message;
        }, function (result) {
            console.log(result);
            vm.message = 'Der opstod en fejl.';
        });
    }

    $scope.disabled = false;


    $scope.today = function () {
        $scope.dt = new Date();
    };
    $scope.today();

    $scope.opened = false;

    $scope.clear = function () {
        $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: false
    };

    $scope.selectMe = function () {
        alert($scope.model.selectedId);
    }

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'dd-MM-yyyy', 'shortDate'];
    $scope.format = $scope.formats[3];

});