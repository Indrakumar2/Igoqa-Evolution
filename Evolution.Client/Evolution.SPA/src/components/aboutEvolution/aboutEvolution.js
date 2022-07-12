import React, { Component } from 'react';
import Logo from '../../assets/images/logo.png';
import { configuration } from '../../appConfig';
class AboutEvolution extends Component {
    render() {
        const { systemSettingData } = this.props;
        const releaseNoteText = Array.isArray(systemSettingData) && systemSettingData.length > 0
            ? systemSettingData[0].keyValue : null;

        const releaseNoteUrl = Array.isArray(systemSettingData) && systemSettingData.length > 1
            ? systemSettingData[1].keyValue : null;
        return (
            <div id="about-content" className="center-align">
                <div className="pt-2 aboutLogo"><img src={Logo} alt="Intertek Logo" /></div>
                <div className="pb-0">
                    {/* <div>Evo ver 2.0</div>
                <div>Evo QC release ver 1.8</div>
                <div>Evolution Helpdesk: +44 (1444) 472909</div> */}
                    <div>Version: {configuration.version}</div>
                    <div>Support Email: <a href={"mailto:industry.support@intertek.com"} className="link">industry.support@intertek.com</a></div>
                    <div className="pb-1">{releaseNoteText}</div>
                    <div><a className="link" target="_blank" href={releaseNoteUrl}>{releaseNoteUrl}</a></div>
                </div>
            </div>
        );
    }
}

export default AboutEvolution;