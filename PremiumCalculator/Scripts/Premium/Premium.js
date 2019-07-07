// premium module
var premiumApp = angular.module('premiumApp', [])

premiumApp.directive('ngSubmit', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});


// premium factory to manage the service calls 
premiumApp.factory('premiumService', ['$http', function ($http) {

    var PremiumService = {};

    // Get available Occupation values
    PremiumService.getOccupations = function () {
        return $http.get('/Premium/GetOccupations');
    };

    // Calculate monthly premium
    PremiumService.calculateMonthlyPremium = function (premium) {
        return $http.post('/Premium/CalculatePremium', { premium: premium });
    };

    return PremiumService;
}]);

// premium controller to manage the various web api calls 
premiumApp.controller('premiumController', function ($scope, $filter, premiumService) {

    $scope.premium = {
        name: '',
        age: '',
        dob: '',
        occupationName: '',
        occupationID: '',
        deathSumInsured: '',
        calculatedPremium: ''
    };

    $scope.occupation = '';
    $scope.isSubmitted = false;
    $scope.toggleMonthlyPremium = false;
    $scope.maxDate = new Date();
    $scope.date = $filter('date')($scope.maxDate, "dd/MM/yyyy");
    $scope.dateAsDate = new Date($scope.maxDate);

    // store the available occupations
    $scope.occupationItems = [];

    // to restore the default values of the controls
    $scope.defaultPremium = angular.copy($scope.premium);

    // reset the controls in the form
    $scope.clearControls = function () {
        $scope.premium = angular.copy($scope.defaultPremium);
        $scope.isSubmitted = false;
    };

    // log the error message
    var onError = function (data, status) {
        alert('Error occured.');
    };

    // get the available occupations
    // bind the model to populate the dropdownlist
    $scope.getOccupationList = function () {
        premiumService.getOccupations()
            .then(function (occupationResult) {
                if (occupationResult != null && occupationResult.data != null) {
                    $scope.occupationItems = occupationResult.data;
                }
            },
                function (data, status) {
                    onError(data, status);
                });
    };

    // calculate the premium based on the selected occupation 
    $scope.manageOccupationChangeEvent = function (form) {
        $scope.toggleMonthlyPremium = false;

        if (!form.$valid) {
            return;
        }

        $scope.isSubmitted = true;

        // calculate the premium
        calculatePremium();
    };

    // calculate the premium based on the selected occupation 
    $scope.calculateMonthlyPremium = function (form) {
        $scope.toggleMonthlyPremium = false;
        $scope.isSubmitted = true;

        if (!form.$valid) {
            return;
        }

        // calculate the premium
        calculatePremium();
    };

    calculatePremium = function () {
        
        $scope.premium.occupationName = $scope.occupation.Name;
        $scope.premium.occupationID = $scope.occupation.ID;

        premiumService.calculateMonthlyPremium($scope.premium)
            .then(function (result) {
                $scope.premium.calculatedPremium = result.data;
                $scope.toggleMonthlyPremium = true;
                angular.element(document.querySelector('#divMonthlyPremium')).text(result.data);
            },
            function (data, status) {
                onError(data, status);
            });
    };

    // bind model with occupations to bind the dropdownlist
    $scope.getOccupationList();
});