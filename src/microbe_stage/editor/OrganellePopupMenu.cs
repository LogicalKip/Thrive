using System;
using System.Globalization;
using Godot;

/// <summary>
///   Manages a custom context menu solely for showing list of options for a placed organelle
///   in the microbe editor.
/// </summary>
public class OrganellePopupMenu : PopupPanel
{
    [Export]
    public NodePath SelectedOrganelleNameLabelPath;

    [Export]
    public NodePath DeleteButtonPath;

    [Export]
    public NodePath MoveButtonPath;

    private Label selectedOrganelleNameLabel;
    private Button deleteButton;
    private Button moveButton;

    private OrganelleTemplate selectedOrganelle;
    private bool enableDelete = true;
    private bool enableMove = true;

    [Signal]
    public delegate void DeletePressed();

    [Signal]
    public delegate void MovePressed();

    /// <summary>
    ///   The placed organelle to be shown options of.
    /// </summary>
    public OrganelleTemplate SelectedOrganelle
    {
        get => selectedOrganelle;
        set
        {
            selectedOrganelle = value;
            UpdateOrganelleNameLabel();
        }
    }

    public bool EnableDeleteOption
    {
        get => enableDelete;
        set
        {
            enableDelete = value;
            UpdateDeleteButton();
        }
    }

    public bool EnableMoveOption
    {
        get => enableMove;
        set
        {
            enableMove = value;
            UpdateMoveButton();
        }
    }

    public override void _Ready()
    {
        selectedOrganelleNameLabel = GetNode<Label>(SelectedOrganelleNameLabelPath);
        deleteButton = GetNode<Button>(DeleteButtonPath);
        moveButton = GetNode<Button>(MoveButtonPath);

        UpdateOrganelleNameLabel();
        UpdateDeleteButton();
        UpdateMoveButton();
    }

    private void OnDeletePressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        EmitSignal(nameof(DeletePressed));

        Hide();
    }

    private void OnMovePressed()
    {
        EmitSignal(nameof(MovePressed));

        Hide();
    }

    private void OnModifyPressed()
    {
        throw new NotImplementedException();
    }

    private void UpdateOrganelleNameLabel()
    {
        if (selectedOrganelleNameLabel == null)
            return;

        selectedOrganelleNameLabel.Text = SelectedOrganelle?.Definition.Name;
    }

    private void UpdateDeleteButton()
    {
        if (deleteButton == null)
            return;

        var mpLabel = deleteButton.GetNode<Label>("MarginContainer/HBoxContainer/MpCost");

        mpLabel.Text = (SelectedOrganelle != null && SelectedOrganelle.PlacedThisSession ?
            "+" + SelectedOrganelle.Definition.MPCost :
            "-" + Constants.ORGANELLE_REMOVE_COST) + " MP";

        deleteButton.Disabled = !EnableDeleteOption;
    }

    private void UpdateMoveButton()
    {
        if (moveButton == null)
            return;

        var mpLabel = moveButton.GetNode<Label>("MarginContainer/HBoxContainer/MpCost");

        mpLabel.Text = "-" + (SelectedOrganelle != null && SelectedOrganelle.MovedThisSession ?
            "0" :
            Constants.ORGANELLE_MOVE_COST.ToString(CultureInfo.CurrentCulture)) + " MP";

        moveButton.Disabled = !EnableMoveOption;
    }
}
