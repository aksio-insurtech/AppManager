// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Remove } from 'API/applications/Remove';
import { Application as ApplicationModel } from 'API/applications/Application';
import { useNavigate } from 'react-router-dom';
import { ResourcesForApplication } from 'API/applications/ResourcesForApplication';
import { AllSettings } from 'API/organization/settings/AllSettings';

export interface IGeneralProps {
    application: ApplicationModel;
}

export const General = (props: IGeneralProps) => {
    const removeApplicationCommand = new Remove();
    const navigate = useNavigate();
    const [settings] = AllSettings.use();
    const [resources] = ResourcesForApplication.use({ applicationId: props.application.id });

    const subscription = settings.data?.azureSubscriptions?.find(_ => _.subscriptionId === resources.data?.azure?.subscriptionId);

    return (
        <></>
        // <Stack>
        //     <CommandBar items={commandBarItems} />

        //     <h1>General</h1>
        //     <TextField label="Name" readOnly value={props.application.name} />

        //     <h1>Azure</h1>
        //     <Link target='_blank' href={`https://portal.azure.com/#@${subscription?.tenantName}/resource${resources.data?.azure?.resourceGroupId}/overview`}>Resource Group</Link>
        //     <Link target='_blank' href={`https://portal.azure.com/#@${subscription?.tenantName}/resource${resources.data?.azure?.resourceGroupId}/providers/Microsoft.Storage/storageAccounts/${resources.data?.azure?.storageAccountName}/overview`}>Storage Account</Link>

        //     <h1>Azure Container Registry</h1>
        //     <TextField label="Login Server" value={resources.data?.azure?.containerRegistryLoginServer || ''} readOnly disabled />
        //     <TextField label="UserName" value={resources.data?.azure?.containerRegistryUserName || ''} readOnly disabled />
        //     <TextField label="Password" value={resources.data?.azure?.containerRegistryPassword || ''} readOnly disabled type="password" canRevealPassword />

        //     <h1>MongoDB</h1>

        //     <TextField label="Server" value={resources.data?.mongoDB?.connectionString || ''} readOnly disabled />

        //     <h3>Users</h3>
        //     {resources.data?.mongoDB?.users?.map(user => {
        //         return (
        //             <Stack key={user.userName} horizontal>
        //                 <TextField label="Username" value={user.userName} />
        //                 <TextField label="Password" value={user.password} readOnly disabled type="password" canRevealPassword />
        //             </Stack>
        //         );
        //     })}
        // </Stack>
    );
};
