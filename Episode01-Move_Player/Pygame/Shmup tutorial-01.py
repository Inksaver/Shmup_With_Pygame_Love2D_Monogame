# https://www.youtube.com/watch?v=nGufy7weyGY
'''
Only 1 player so use code module (static class) for player
and a separate shared code module for global variables
'''
# import libraries
import pygame, os
import shared, player

def process_events() -> (object, object, bool):
	''' get keyboard input and check if user closed window '''
	key_down = None
	quit = False
	for event in pygame.event.get():				# process events
		if event.type == pygame.QUIT:				# close button clicked
			quit = True
		elif event.type == pygame.KEYDOWN:			# single press of any key
			key_down = event.key	

	keystate = pygame.key.get_pressed()				# get keypressed events
	if keystate[pygame.K_ESCAPE]:					# player pressing escape continuously
		quit = True

	return keystate, key_down, quit					# usage: if key_down == pygame.K_RETURN:, if keystate[pygame.K_UP]:

def load() -> None:
	''' Setup pygame and load all assets '''
	global player
	shared.game_folder = os.getcwd() 					# current directory
	os.environ["SDL_VIDEO_CENTERED"] = "1" 				# Centre the Pygame window on screen
	pygame.init()       								# initialise pygame and game window
	try:
		pygame.mixer.init() 							# start pygame sound library
	except:
		shared.audio_present = False					# audio driver not installed
	shared.screen = pygame.display.set_mode((shared.WIDTH, shared.HEIGHT)) 
	pygame.display.set_caption(shared.window_title) 	# The window title
	shared.clock = pygame.time.Clock() 					# Keep track of framerate etc

	player.init(40, 50, 500, shared.GREEN)	# create a player (50x40 green rectangle)


def update() -> None:
	'''
	Update all game items and keyboard input
	delta-time used in Love2D and Monogame is measured in seconds
	Can be obtained from clock.tick() in ms
	'''
	dt = shared.clock.tick(shared.FPS) / 1000				# update clock. dt can be passed to other update functions
	keystate, key_down, quit = process_events()				# get keypressed, keydown and close/esc user choice
	if quit:
		shared.gamestate = shared.gamestates["quit"]		# set gamestate to quit
	else:
		if shared.gamestate == shared.gamestates["play"]:
			#shared.player.update(keystate, dt)
			player.update(keystate, dt)

def draw() -> None:
	''' Draw background and all active sprites '''
	shared.screen.fill(shared.BLACK)					# make screen black
	if shared.gamestate == shared.gamestates["play"]:
		#shared.player.draw()
		player.draw()

	pygame.display.flip()								# flip display to make it visible

def main() -> None:
	''' Run game loop and call other functions from here '''
	shared.WIDTH = 480									# default screen width: alter as required
	shared.HEIGHT = 600									# default screen height: alter as required
	shared.window_title = "Shmup!"						# default window title: change as required	
	load()												# setup window and game assets

	''' gameloop '''
	shared.gamestate = shared.gamestates["play"]		# gamestate starting at 1 ('play': no menu)
	while shared.gamestate < shared.gamestates["quit"]:	# 3 = quit
		update()										# go through update functions
		draw()											# go through draw functions

	pygame.quit()

main()