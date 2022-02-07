''' 
Only one leaderboard, so should be static class, but Python code module
is not up to the job, so full class used instead.
'''
import os, pygame, csv
import shared, player

class ScoreData():
	''' internal class for Leaderboard use only'''
	def __init__(self, line:list):
		''' line is a list or tuple '''
		self.name = line[0]
		self.score = int(line[1])
		
	def get_score_data(self, dest:str) -> str:
		''' returns a formatted string for writing to file OR displaying onscreen '''
		if dest == "file":
			return f"{self.name};{self.score}\n"	# 'FRED;2500\n'
		else:
			return f"{self.name} : {self.score}"	# 'FRED : 2500'

class Leaderboard():	
	def __init__(self) -> None:
		self.current_player_entered:bool = False	# Has player entered a name?
		self.score_list:list = []					# list of ScoreData objects
		self.name:str = ""							# player name
		self.ypos:int = 80							# y coordinate of leaderboard display
		pygame.font.init()							# initialise fonts
		self.populate_score_list()
	
	def add_entry(self) -> None:
		temp_score_data = ScoreData((self.name, shared.score)) # pass tuple (or list)
		self.insert_data(temp_score_data)			# put the score in the correct order
	
	def add_score_data(self, line) -> None:
		'''
		could be done as a single line:
		self.insert_data(ScoreData(line))
		'''
		temp_score_data = ScoreData(line) 			# create new ScoreData object
		self.insert_data(temp_score_data)			# insert into correct place in score_list
		
	def populate_score_list(self) -> None:
		self.score_list.clear()						# remove all items in score_list
		self.name = ""
		shared.score = 0							# reset score to 0
		if os.path.exists(os.path.join(shared.game_folder,"Highscore.txt")):
			leader = open(os.path.join(shared.game_folder,"Highscore.txt"), "r")
			c = csv.reader(leader, delimiter=';', skipinitialspace = True)
			# fill list with scoreData objects in high -> low order
			for line in c:							# eg: INKSAVER;4904
				if len(line) > 0:					# check not empty line
					self.add_score_data(line)
			leader.close()
		
	def insert_data(self, score_data) -> None:
		''' Insert a name/score into score_list, highscore at top '''
		insert_at = len(self.score_list)				# assume insert at end of list
		for i in range(len(self.score_list)):			# iterate list
			if score_data.score >= self.score_list[i].score: # new score is > current list item
				insert_at = i							# re-define insertion point
				break									# break loop
		self.score_list.insert(insert_at, score_data)	# add ScoreData object to list
				
	def write_score_list(self) -> None:
		''' Write top 6 scores into text file, over-write original '''
		lines = 0
		leader = open(os.path.join(shared.game_folder,"Highscore.txt"), "w")
		for i in range(len(self.score_list)):
			if lines < 5:
				leader.write(self.score_list[i].get_score_data("file"))
			lines += 1
			
		leader.close()
			
	def update(self, keystate:object, key_down:object) -> None:
		if not self.current_player_entered: 			# If name not entered get input from user
			if key_down != None:						# have we got a key_down ?
				if key_down == pygame.K_BACKSPACE:		# backspace so delete a character
					if len(self.name) > 1:
						self.name = self.name[0:-1]		# remove end character
					elif len(self.name) == 1:			# 1 remaining -> empty string
						self.name = ""
				elif key_down == pygame.K_RETURN:		# Enter key
					if len(self.name.strip()) >= 1:		# at least 1 character
						self.current_player_entered = True
						self.add_entry()
				elif key_down <= 127:					# any ascii character
					if len(self.name) < 15:				# limit name to 15 chars
						self.name += chr(key_down).upper()
		
		else: # name already obtained. waiting for c to continue
			if key_down == pygame.K_c:							# c key pressed
				shared.gamestate = shared.gamestates['menu']	# set menu gamestate
				self.current_player_entered = False				# reset flag
			
	def draw(self) -> None:
		# draw the word Leaderboard at y = 50, centre window, in yellow
		shared.draw_text(shared.screen, "LEADERBOARD", 50, shared.WIDTH * 0.5, 0, "centre", shared.YELLOW)
		# draw a list of 5 names with current name in position related to score
		self.ypos = 80		
		for i in range(len(self.score_list)):# up to 5 highscore names and scores
			shared.draw_text(shared.screen, f"{self.score_list[i].get_score_data('display')}",
							 20, shared.WIDTH * 0.5, self.ypos, "centre")
			self.ypos += 30				# move down 30 pixels
			
		if self.current_player_entered: # If got name input from user
			shared.draw_text(shared.screen, "Press C to continue", 18, shared.WIDTH / 2, shared.HEIGHT * 0.8)
			self.write_score_list()
			self.populate_score_list()
		else:							# still waiting for user to input their name
			shared.draw_text(shared.screen, "Type your name and press Enter", 18, shared.WIDTH * 0.5, shared.HEIGHT * 0.8)
			shared.display_box(shared.screen, f"NAME: {self.name}", 18,
							   pygame.Rect(50, shared.HEIGHT * 0.85, shared.WIDTH - 100, 24))
