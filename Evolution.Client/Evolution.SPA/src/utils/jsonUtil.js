export const getInitialUserDetailJson = () => {
    return {
        user: {
            userName: '',
            logonName: '',
            email: '',
            phoneNumber: '',
            companyCode: '',
            companyOfficeName: '',
            authenticationMode: 'AD',
            userType: '',
            isActive: true,
            culture: '',
            recordStatus: 'N'
        },
        companyRoles: [],
        companyUserTypes: []
    };
};

export const getRoleInitialJson = () => {
    return {
        roleName: null,
        description: null,
        recordStatus: null,
        isSelected: false,
        companyCode: null
    };
};

export const getUserTypesJsonArray = () => {
    return [
      //  { value: 'ProjectCoordinator', name: 'Project Coordinator' }, // Changes done as per ITK requirement(D731 issue2) --- //D669 Changes (ref mail RE:D669 on 18-02-2020)
        { value: 'Customer', name: 'Customer' },
        { value: 'MICoordinator', name: 'ITK Coordinator' }, // Changes done as per ITK requirement
        { value: 'OperationManager', name: 'Operation Manager' },
        { value: 'ResourceCoordinator', name: 'Resource Coordinator' },
       // { value: 'ResourceManager', name: 'Resource Manager' }, //D669 done as per ITK Requirement (11-02-2020 mail doc)
     //   { value: 'SeniorCoordinator', name: 'Senior Coordinator' }, // Commented done as per ITK requirement
        { value: 'TechnicalManager', name: 'Technical Manager' },
        { value: 'TechnicalSpecialist', name: 'Resource' }
    ];
};