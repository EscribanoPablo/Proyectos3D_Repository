public class PlayerControllerReference : IRestartLevelElement
{
    private PlayerController playerController;

    public PlayerControllerReference(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Restart()
    {
        // No necesitamos hacer nada aquí, ya que solo estamos manteniendo una referencia al PlayerController
    }
}
