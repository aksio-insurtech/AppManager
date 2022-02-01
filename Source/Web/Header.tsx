// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Dropdown, IDropdownOption } from '@fluentui/react';
import { AllOrganizations } from 'API/organizations/AllOrganizations';
import { useState } from 'react';

export const Header = () => {
    const [organizations] = AllOrganizations.use();
    const [selectedOrganization, setSelectedOrganization] = useState<string>();

    const options: IDropdownOption[] = organizations.data.map(_ => {
        return {
            key: _.id,
            text: _.name
        };
    });

    if( !selectedOrganization && organizations.data.length > 0 ) {
        setSelectedOrganization(organizations.data[0].id);
    }

    return (
        <div>
            <Dropdown
                label="Organization"
                options={options}
                selectedKey={selectedOrganization}
                />
        </div>
    );
};
