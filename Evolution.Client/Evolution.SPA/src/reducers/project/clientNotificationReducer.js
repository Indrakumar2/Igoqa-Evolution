import { projectActionTypes } from '../../constants/actionTypes';

export const ClientNotificationReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case projectActionTypes.clientNotification.IS_CLIENT_NOTIFICATION_EDIT:
            state = {
                ...state,
                isClientNotificationEdit: data
            };
            return state;

        case projectActionTypes.clientNotification.DELETE_CLIENT_NOTIFICATION:
            state = {
                ...state,
                projectDetail:{
                    ...state.projectDetail,
                    ProjectNotifications:data
                },
                isbtnDisable: false
            };
            return state;

        case projectActionTypes.clientNotification.ADD_CLIENT_NOTIFICATION:
            if (state.projectDetail.ProjectNotifications == null) {
                state = {
                    ...state,
                    projectDetail: {
                        ...state.projectDetail,
                        ProjectNotifications: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                projectDetail:{
                    ...state.projectDetail,
                    ProjectNotifications:[ ...state.projectDetail.ProjectNotifications,data ]
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.clientNotification.EDITED_CLIENT_NOTIFICATION_DATA:
            state = {
                ...state,
                editedClientNotificationData:data
            };
            return state;

        case projectActionTypes.clientNotification.UPDATE_CLIENT_NOTIFICATION_DATA:

            state = {
                ...state,
                projectDetail:{
                    ...state.projectDetail,
                    ProjectNotifications:data
                },
                isbtnDisable: false
            };
            return state;
        case projectActionTypes.FETCH_CUSTOMER_CONTACT:
            state = {
                ...state,
                customerContact:data,                
            };
            return state;

        default:
            return state;
        }
    };