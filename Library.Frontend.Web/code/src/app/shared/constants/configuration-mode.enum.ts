enum ConfigurationMode {
  Create = 'create',
  Edit = 'edit',
}

// [GET NAME] => configurationModeName.get(ConfigurationMode.Create)
const configurationModeName = new Map<ConfigurationMode, string>([
  [ConfigurationMode.Create, 'Skapa'],
  [ConfigurationMode.Edit, 'Ändra'],
]);

export { ConfigurationMode, configurationModeName };

