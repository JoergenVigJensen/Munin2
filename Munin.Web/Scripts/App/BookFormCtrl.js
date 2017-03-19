muninApp.controller('booksFormCtrl', function ($scope, $timeout, $http) {

    //https://angular-ui.github.io/ui-select/

    var vm = this;

    vm.materialer = [];
    vm.journaler = [];
    vm.model = {};

    vm.journal = {};
    vm.materiale = {};

    $scope.dataset = [];
    $scope.selectedItem = {};
    $scope.pageLoaded = false;

    $scope.initpage = function () {
        var id = angular.element('#ID').val();
        $http.get('/books/load/' + id).then(function (result) {
            console.log(result);
            vm.model = result.data.model;
            if (result.data.model.journal != null) {
                vm.journal.value = vm.model.journal.journalId;
                vm.journal.text = vm.model.journal.journalNb;
            }
            vm.journaler = result.data.journalList;
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
        if (vm.materiale != null) {
            vm.model.pictureMaterial = vm.materiale.value;
        }
        $http.post('/books/save/', vm.model).then(function (result) {
            if (result.data.success === true)
                window.location.href = "/books/index";
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