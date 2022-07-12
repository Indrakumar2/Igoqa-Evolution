import React, { Component, Fragment } from 'react';
import { getlocalizeData, isEmptyOrUndefine } from '../../../utils/commonUtils';
import Modal from '../modal';
const localConstant = getlocalizeData();
export const AnnouncementHeader = (props) => {
    return (
        <Fragment>
            {
                !isEmptyOrUndefine(props.annoncementData) && props.annoncementData.length > 0 ? <span onClick={props.anouncementClick} className="pointer">{props.annoncementData[0].header}</span> : null
            }
        </Fragment>
    );
};

export const AnnouncementDesc = (props) => {
    return (
        <p>{props.announcementDescription}</p>
    );
};
class Announcement extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isAnnoncementOpen: false,
            announcementDesc: ""
        };
        this.annoncementButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.annoncementCloseClick,
                btnID: "closeAnnouncementList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
        ];
    }
    annoncementClick = (e) => {
        e.preventDefault();
        let annoncementDescription = "";
        if (Array.isArray(this.props.annoncementData) && this.props.annoncementData.length > 0) {
            this.props.annoncementData.map(annoncement => {
                if (annoncement.header === e.target.textContent)
                    annoncementDescription = annoncement.text;
            });
        }
        this.setState({ isAnnoncementOpen: true, announcementDesc: annoncementDescription });
    }
    annoncementCloseClick = (e) => {
        e.preventDefault();
        this.setState({ isAnnoncementOpen: false });
    }
    //need to be changed in 2nd phase(Hard coded now)
    announcementColourPicker = (colourCode) => {
        switch (colourCode) {
            case "-16777216":
                return "black";
            case "-16776961":
                return "blue";
            case "-16744448":
                return "green";
            case "-65536":
                return "red";
            case "-256":
                return "yellow";
            case "-32768":
                return "orange";
            case "-1":
                return "white";
            default:
                return "green";
        }
    }
    render() {
        const { annoncementData } = this.props;
        return (
            <Fragment>
                <div className={this.props.className} style={{ backgroundColor: this.announcementColourPicker(annoncementData[0].backgroundColour), color: this.announcementColourPicker(annoncementData[0].textColour) }}>
                    <marquee className="bold" behavior="scroll" direction="left" scrollamount="3">
                        <AnnouncementHeader annoncementData={annoncementData} anouncementClick={this.annoncementClick} />
                    </marquee>
                </div>
                {
                    this.state.isAnnoncementOpen ?
                        <Modal title={"Announcement"}
                            modalId="announcementPopup"
                            formId="announcementForm"
                            buttons={this.annoncementButtons}
                            modalClass={"annoncementModal"}
                            isShowModal={true}
                        >
                            {/* <Announcement annoncementData={annoncementData} isAnnouncementPopup={true}/> */}
                            <AnnouncementDesc announcementDescription={this.state.announcementDesc} />
                        </Modal> : null
                }
            </Fragment>
        );
    }
}

export default Announcement;