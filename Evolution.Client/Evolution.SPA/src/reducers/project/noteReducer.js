import { projectActionTypes } from '../../constants/actionTypes';

export const ProjectNotes = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case projectActionTypes.projectNotes.ADD_PROJRCT_NOTE:
            if (state.projectDetail.ProjectNotes == null) {
                state = {
                    ...state,
                    projectDetail: {
                        ...state.projectDetail,
                        ProjectNotes: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                projectDetail: {
                    ...state.projectDetail,
                    ProjectNotes: [
                        ...state.projectDetail.ProjectNotes, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;
            case projectActionTypes.projectNotes.EDIT_PROJRCT_NOTE: //D661 issue8
                    state = {
                        ...state,
                        projectDetail: {
                            ...state.projectDetail,
                            ProjectNotes: data
                        },
                        isbtnDisable: false
                    };
            return state;
        default:
            return state;
    }
};