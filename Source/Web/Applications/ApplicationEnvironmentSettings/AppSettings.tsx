// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import Editor from '@monaco-editor/react';
import { Box, Button, Toolbar } from '@mui/material';
import { useEffect, useState } from 'react';
import * as icons from '@mui/icons-material';
import { AppSettingsForApplicationEnvironmentId } from 'API/applications/environments/AppSettingsForApplicationEnvironmentId';
import { useRouteParams } from '../RouteParams';
import { SetAppSettingsForApplicationEnvironment } from 'API/applications/environments/SetAppSettingsForApplicationEnvironment';
import { useSnackbar } from 'notistack';

export const AppSettings = () => {
    const { applicationId, environmentId } = useRouteParams();
    const { enqueueSnackbar } = useSnackbar();
    const [appSettings] = AppSettingsForApplicationEnvironmentId.use({ environmentId: environmentId! });
    const [saveCommand, setSaveCommandValues] = SetAppSettingsForApplicationEnvironment.use();

    const [content, setContent] = useState('{}');
    const [initialContent, setInitialContent] = useState('{}');
    const [isDirty, setIsDirty] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        const actualContent = appSettings.data?.content ?? '{}';
        setInitialContent(actualContent);
        setContent(actualContent);
        setIsDirty(false);
    }, [appSettings.data]);

    const save = async () => {
        try {
            JSON.parse(content);
        }
        catch (ex) {
            enqueueSnackbar('Invalid JSON', { variant: 'error' });
            return;
        }

        setSaveCommandValues({
            applicationId: applicationId!,
            environmentId: environmentId!,
            content
        });
        const result = await saveCommand.execute();
        if (result.isSuccess) {
            enqueueSnackbar('Saved', { variant: 'success' });
            setInitialContent(content);
            setIsDirty(false);
        } else {
            enqueueSnackbar(`Failed to save - ${result.exceptionMessages}`, { variant: 'error' });
        }
    };

    const contentChanged = (json: string) => {
        setIsLoading(false);
        if (isLoading) return;

        setIsDirty(json !== initialContent);
        setContent(json);
    };

    return (
        <Box sx={{ height: '100%', flex: 1 }}>

            <Toolbar>
                <Button startIcon={<icons.Save />} onClick={save} disabled={!isDirty}>Save</Button>
            </Toolbar>

            <Editor
                height="45vh"
                theme="vs-dark"
                defaultValue="{}"
                value={content}
                language="JSON"
                defaultLanguage="JSON"
                onChange={(json) => contentChanged(json!)}
            />
        </Box>
    );
};
