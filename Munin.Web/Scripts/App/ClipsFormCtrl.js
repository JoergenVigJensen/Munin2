muninApp.controller('clipsFormCtrl', function ($scope, $timeout, $http) {

    var vm = this;

    vm.materialer = [];
    vm.journaler = [];
    vm.model = {};


    $scope.dataset = [];
    $scope.selectedItem = {};
    $scope.pageLoaded = false;

    var getPaper = function (paperId) {
        for (i = 0; i < vm.papers.length; i++) {
            if (vm.papers[i].value === paperId)
                return vm.papers[i];
        }
        return vm.materialer[0];
    }

    $scope.initpage = function () {
        var id = angular.element('#ID').val();
        $http.get('/clips/load/' + id).then(function (result) {
            console.log(result);
            vm.model = result.data.model;
            vm.model.dateTime = new Date(result.data.model.dateTime.split('T')[0]);
            vm.papers = result.data.papers;
            vm.paper = getPaper(result.data.model.paper);
            $scope.pageLoaded = true;
        },
            function (result) {
                vm.message = "Der opstod en fejl i forbindelse med at vise siden.";
                console.log(result);
            });
    }

    $scope.save = function () {
        vm.model.paper = vm.paper.value;
        $http.post('/clips/save/', vm.model).then(function (result) {
            if (result.data.success === true)
                window.location.href = "/clips/index";
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