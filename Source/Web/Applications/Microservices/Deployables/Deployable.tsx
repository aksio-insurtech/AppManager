// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Pivot, PivotItem, Stack } from '@fluentui/react';

export const Deployable = () => {
    return (
        <Stack style={{ height: '100%' }}>
            <Stack.Item>
                <Pivot>
                    <PivotItem headerText='General'>
                    </PivotItem>
                    <PivotItem headerText='Environment'>
                    </PivotItem>
                    <PivotItem headerText='Secrets'>
                    </PivotItem>
                </Pivot>
            </Stack.Item>
        </Stack>
    );
};
